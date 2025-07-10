using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACUnified.Business.Services
{
    public class ApplicantPaymentMigrationService : IApplicantPaymentMigrationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IApplicantDebtorListRepository _debtorListRepository;

        public ApplicantPaymentMigrationService(
            ApplicationDbContext context,
            IApplicantDebtorListRepository debtorListRepository)
        {
            _context = context;
            _debtorListRepository = debtorListRepository;
        }

        private async Task<decimal> GetTotalPaymentsByApplicationId(long applicationFormId)
        {
            try 
            {
                return await _context.ApplicantPayment
                    .Where(p => p.ApplicationFormId == applicationFormId 
                        && p.ApplicantPayCategoryId >= 9 
                        && p.ApplicantPayCategoryId <= 53
                        && p.isSuccessful
                        && p.Amount > 0)
                    .Select(p => p.Amount)
                    .SumAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating total payments: {ex.Message}");
                return decimal.Zero;
            }
        }

        public async Task<List<ApplicantDebtorListDto>> MigratePaymentsToDebtorListAsync(
            int startId = 9, 
            int endId = 53, 
            Action<int> progressCallback = null)
        {
            try
            {
                var payments = await _context.ApplicantPayment
                    .Include(p => p.ApplicantPayCategory)
                    .Include(p => p.ApplicantPayDetails)
                        .ThenInclude(af => af.ApplicantPayCategory)
                    .Include(p => p.ApplicationForm)
                        .ThenInclude(af => af.BioData)
                    .Include(p => p.ApplicationForm)
                        .ThenInclude(af => af.Level)
                    .Include(p => p.Semester)                
                    .Where(p => p.ApplicantPayCategory != null)
                    .Where(p => p.ApplicantPayCategoryId >= startId && p.ApplicantPayCategoryId <= endId)
                    .ToListAsync();

                var results = new List<ApplicantDebtorListDto>();
                var processedCount = 0;

                foreach (var payment in payments)
                {
                    if (payment.ApplicationFormId == null)
                    {
                        processedCount++;
                        progressCallback?.Invoke(processedCount);
                        continue;
                    }

                    try
                    {
                        decimal totalAmount = payment.ApplicantPayDetails?.ApplicantPayCategory?.TotalPayDue ?? decimal.Zero;
                        decimal totalPaid = await GetTotalPaymentsByApplicationId(payment.ApplicationFormId.Value);
                        decimal outstandingAmount = totalAmount - totalPaid;

                        // Calculate the outstanding percentage
                        decimal outstandingPercentage = totalAmount > 0 
                            ? (outstandingAmount / totalAmount) * 100 
                            : 0;

                        // Determine payment plan based on outstanding percentage
                        PayPlan determinedPaymentPlan;
                        if (outstandingAmount <= 0)
                        {
                            determinedPaymentPlan = PayPlan.Full;
                        }
                        else if (outstandingPercentage <= 25) // 75% paid
                        {
                            determinedPaymentPlan = PayPlan.SecondInstalment;
                        }
                        else if (outstandingPercentage <= 50) // 50% paid
                        {
                            determinedPaymentPlan = PayPlan.FirstInstalment;
                        }
                        else 
                        {
                            determinedPaymentPlan = PayPlan.Full;
                        }

                        var debtorList = new ApplicantDebtorList
                        {
                            ApplicationFormId = payment.ApplicationFormId.Value,
                            OutstandingAmount = outstandingAmount,
                            DueDate = payment.isSuccessful ? 
                                DateTime.Now.Date.AddDays(-(DateTime.Now.Day - 1)).AddMonths(1).AddDays(-1) :
                                DateTime.Now.Date.AddMonths(1),
                            PaymentPlan = determinedPaymentPlan,
                            SemesterId = payment.SemesterId,                    
                            InstitutionProviderId = payment.ApplicationForm?.InstitutionProviderId,
                            CreatedDate = DateTime.Now,                    
                        };

                        var createdDto = await _debtorListRepository.CreateAsync(debtorList);
                        if (createdDto != null)
                        {
                            results.Add(createdDto);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing payment {payment.Id}: {ex.Message}");
                    }
                    finally
                    {
                        processedCount++;
                        progressCallback?.Invoke(processedCount);
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in migration: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }

    // Extension method for dependency injection
    public static class ApplicantPaymentMigrationServiceExtensions
    {
        public static IServiceCollection AddApplicantPaymentMigrationService(this IServiceCollection services)
        {
            services.AddScoped<IApplicantPaymentMigrationService, ApplicantPaymentMigrationService>();
            return services;
        }
    }
}