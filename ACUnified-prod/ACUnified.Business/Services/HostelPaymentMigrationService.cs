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
    public class HostelPaymentMigrationService : IHostelPaymentMigrationService
    {
        private const int HOSTEL_PAY_CATEGORY_ID = 60;
        private const decimal SECOND_INSTALLMENT_THRESHOLD = 25;
        private const decimal FIRST_INSTALLMENT_THRESHOLD = 50;

        private readonly ApplicationDbContext _context;
        private readonly IHostelDebtorListRepository _debtorListRepository;

        public HostelPaymentMigrationService(
            ApplicationDbContext context,
            IHostelDebtorListRepository debtorListRepository)
        {
            _context = context;
            _debtorListRepository = debtorListRepository;
        }

       private async Task<decimal> GetTotalPaymentsByApplicationId(long applicationFormId)
{
    try 
    {
        var sum = await _context.ApplicantPayment
            .Where(p => p.ApplicationFormId == applicationFormId 
                && p.ApplicantPayCategoryId == HOSTEL_PAY_CATEGORY_ID
                && p.isSuccessful
                && p.Amount > 0)
            .Select(p => p.Amount)
            .SumAsync();
            
        return sum;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error calculating total payments: {ex.Message}");
        return decimal.Zero;
    }
}

        private PayPlan DeterminePaymentPlan(decimal outstandingPercentage) =>
            outstandingPercentage <= 0 ? PayPlan.Full :
            outstandingPercentage <= SECOND_INSTALLMENT_THRESHOLD ? PayPlan.SecondInstalment :
            outstandingPercentage <= FIRST_INSTALLMENT_THRESHOLD ? PayPlan.FirstInstalment : 
            PayPlan.Full;

        private DateTime CalculateDueDate(bool isSuccessful) =>
            isSuccessful ? 
                DateTime.Now.Date.AddDays(-(DateTime.Now.Day - 1)).AddMonths(1).AddDays(-1) :
                DateTime.Now.Date.AddMonths(1);

        private async Task<HostelDebtorList> CreateDebtorListEntry(ApplicantPayment payment)
        {
            decimal totalAmount = payment.ApplicantPayDetails?.ApplicantPayCategory?.TotalPayDue ?? 0;
            decimal totalPaid = await GetTotalPaymentsByApplicationId(payment.ApplicationFormId.Value);
            decimal outstandingAmount = totalAmount - totalPaid;
            decimal outstandingPercentage = totalAmount > 0 ? (outstandingAmount / totalAmount) * 100 : 0;

            return new HostelDebtorList
            {
                ApplicationFormId = payment.ApplicationFormId.Value,
                OutstandingAmount = outstandingAmount,
                DueDate = CalculateDueDate(payment.isSuccessful),
                PaymentPlan = DeterminePaymentPlan(outstandingPercentage),
                SemesterId = payment.SemesterId,
                CreatedDate = DateTime.Now
            };
        }

        public async Task<List<HostelDebtorListDto>> MigratePaymentsToDebtorListAsync(
            int startId = HOSTEL_PAY_CATEGORY_ID,
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
                    .Where(p => p.ApplicantPayCategory != null 
                        && p.ApplicantPayCategoryId == startId)
                    .ToListAsync();

                var results = new List<HostelDebtorListDto>();
                var processedCount = 0;

                foreach (var payment in payments.Where(p => p.ApplicationFormId != null))
                {
                    try 
                    {
                        var debtorList = await CreateDebtorListEntry(payment);
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
                        progressCallback?.Invoke(++processedCount);
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

    public static class HostelPaymentMigrationServiceExtensions
    {
        public static IServiceCollection AddHostelPaymentMigrationService(this IServiceCollection services)
        {
            services.AddScoped<IHostelPaymentMigrationService, HostelPaymentMigrationService>();
            return services;
        }
    }
}

