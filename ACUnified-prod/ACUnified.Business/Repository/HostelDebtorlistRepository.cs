using ACUnified.Data.Models;
using ACUnified.Data.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.Enum;

namespace ACUnified.Business.Repository
{
    public class HostelDebtorListRepository : IHostelDebtorListRepository
    {
        private readonly ApplicationDbContext _context;

        public HostelDebtorListRepository(ApplicationDbContext context)
        {
            _context = context;
        }
private async Task<decimal> GetTotalPaymentsByApplicationId(long applicationFormId)
{
    try 
    {
        // Get all successful payments for the application within categories 9-53
        return await _context.ApplicantPayment
            .Where(p => p.ApplicationFormId == applicationFormId 
                && p.ApplicantPayCategoryId == 60
                && p.isSuccessful
                && p.Amount > 0)  // Only include positive amounts
            .Select(p => p.Amount)
            .SumAsync();  // Sum directly in the database
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error calculating total payments: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        return decimal.Zero;
    }
}

        public async Task<IEnumerable<HostelDebtorListDto>> GetAllAsync()
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
                        && p.ApplicantPayCategoryId == 60)
                    .AsNoTracking()
                    .ToListAsync();

                var results = new List<HostelDebtorListDto>();

                foreach (var payment in payments)
                {
                    if (payment.ApplicationFormId == null)
                    {
                        continue;
                    }

                    try
                    {
                        decimal totalAmount = payment.ApplicantPayDetails?.ApplicantPayCategory?.TotalPayDue ?? decimal.Zero;
                        decimal totalPaid = await GetTotalPaymentsByApplicationId(payment.ApplicationFormId.Value);
                        decimal outstandingAmount = totalAmount - totalPaid;
                        decimal outstandingPercentage = totalAmount > 0 
                            ? (outstandingAmount / totalAmount) * 100 
                            : 0;

                        PayPlan determinedPaymentPlan = DeterminePaymentPlan(outstandingAmount, outstandingPercentage);

                        var debtorList = await GetOrCreateDebtorList(
                            payment, 
                            outstandingAmount, 
                            determinedPaymentPlan);

                        results.Add(CreateDebtorListDto(
                            debtorList, 
                            payment, 
                            totalAmount, 
                            totalPaid, 
                            outstandingAmount));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing payment {payment.Id}: {ex.Message}");
                        continue;
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private PayPlan DeterminePaymentPlan(decimal outstandingAmount, decimal outstandingPercentage)
        {
            if (outstandingAmount <= 0)
                return PayPlan.Full;
            else if (outstandingPercentage <= 25)
                return PayPlan.SecondInstalment;
            else if (outstandingPercentage <= 50)
                return PayPlan.FirstInstalment;
            else
                return PayPlan.Full;
        }

        private async Task<HostelDebtorList> GetOrCreateDebtorList(
            ApplicantPayment payment,
            decimal outstandingAmount,
            PayPlan determinedPaymentPlan)
        {
            var debtorList = await _context.HostelDebtorList
                .Include(d => d.Semester)
                // .Include(d => d.Session)
                .Include(d => d.ApplicationForm)
                    .ThenInclude(af => af.BioData)
                .Include(d => d.ApplicationForm)
                    .ThenInclude(af => af.Level)
                // .Include(d => d.InstitutionProvider)
                .FirstOrDefaultAsync(d => d.ApplicationFormId == payment.ApplicationFormId);

            if (debtorList == null)
            {
                debtorList = new HostelDebtorList
                {
                    ApplicationFormId = payment.ApplicationFormId.Value,
                    OutstandingAmount = outstandingAmount,
                    DueDate = payment.isSuccessful
                        ? DateTime.Now.Date.AddDays(-(DateTime.Now.Day - 1)).AddMonths(1).AddDays(-1)
                        : DateTime.Now.Date.AddMonths(1),
                    PaymentPlan = determinedPaymentPlan,
                    SemesterId = payment.SemesterId,
                    // InstitutionProviderId = payment.ApplicationForm?.InstitutionProviderId,
                    CreatedDate = DateTime.Now,
                };

                _context.HostelDebtorList.Add(debtorList);
            }
            else
            {
                debtorList.OutstandingAmount = outstandingAmount;
                debtorList.PaymentPlan = determinedPaymentPlan;
            }

            await _context.SaveChangesAsync();
            return debtorList;
        }

        private HostelDebtorListDto CreateDebtorListDto(
            HostelDebtorList debtorList,
            ApplicantPayment payment,
            decimal totalAmount,
            decimal totalPaid,
            decimal outstandingAmount)
        {
            return new HostelDebtorListDto
            {
                Id = debtorList.Id,
                ApplicationFormId = debtorList.ApplicationFormId,
                OutstandingAmount = outstandingAmount,
                TotalAmount = totalAmount,
                TotalPaid = totalPaid,
                ApplicantPaymentId = payment.Id,
                ApplicantPayDetailsId = payment.ApplicantPayDetailsId,
                DueDate = debtorList.DueDate,
                PaymentPlan = debtorList.PaymentPlan,
                SemesterId = debtorList.SemesterId,
                // SessionId = debtorList.SessionId,
                // InstitutionProviderId = debtorList.InstitutionProviderId,
                ApplicantPayment = payment,
                ApplicantPayDetails = payment.ApplicantPayDetails,
                Semester = debtorList.Semester,
                // Session = debtorList.Session,
                ApplicationForm = debtorList.ApplicationForm
            };
        }

        public async Task<IEnumerable<HostelDebtorListDto>> GetByApplicationFormIdAsync(long applicationFormId)
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
            .Where(p => p.ApplicationFormId == applicationFormId
                && p.ApplicantPayCategoryId == 60)
            .AsNoTracking()
            .ToListAsync();

        var results = new List<HostelDebtorListDto>();

        foreach (var payment in payments)
        {
            try
            {
                decimal totalAmount = payment.ApplicantPayDetails?.ApplicantPayCategory?.TotalPayDue ?? decimal.Zero;
                decimal totalPaid = await GetTotalPaymentsByApplicationId(applicationFormId);
                decimal outstandingAmount = totalAmount - totalPaid;
                decimal outstandingPercentage = totalAmount > 0 
                    ? (outstandingAmount / totalAmount) * 100 
                    : 0;

                PayPlan determinedPaymentPlan = DeterminePaymentPlan(outstandingAmount, outstandingPercentage);

                // Get existing debtor list from database
                var debtorList = await _context.HostelDebtorList
                    .Include(d => d.Semester)
                    
                    .Include(d => d.ApplicationForm)
                        .ThenInclude(af => af.BioData)
                    .Include(d => d.ApplicationForm)
                        .ThenInclude(af => af.Level)
                    
                    .FirstOrDefaultAsync(d => d.ApplicationFormId == applicationFormId);

                if (debtorList != null)
                {
                    results.Add(CreateDebtorListDto(
                        debtorList,
                        payment,
                        totalAmount,
                        totalPaid,
                        outstandingAmount));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing payment {payment.Id}: {ex.Message}");
                continue;
            }
        }

        return results;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GetByApplicationFormIdAsync: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw;
    }
}

             public async Task<HostelDebtorListDto> GetByIdAsync(long id)
        {
            try
            {
                var debtorList = await _context.HostelDebtorList
                    .Include(p => p.ApplicationForm)
                        .ThenInclude(af => af.BioData)
                    .Include(p => p.ApplicationForm)
                        .ThenInclude(af => af.Level)
                    .Include(p => p.Semester)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (debtorList == null)
                    return null;

                var payment = await _context.ApplicantPayment
                    .Include(ap => ap.ApplicantPayDetails)
                        .ThenInclude(apd => apd.ApplicantPayCategory)
                    .Where(ap => ap.ApplicationFormId == debtorList.ApplicationFormId)
                    .OrderByDescending(ap => ap.Id)
                    .FirstOrDefaultAsync();

                if (payment == null)
                    return null;

                decimal totalAmount = payment.ApplicantPayDetails?.ApplicantPayCategory?.TotalPayDue ?? decimal.Zero;
                decimal totalPaid = await GetTotalPaymentsByApplicationId(debtorList.ApplicationFormId);

                return CreateDebtorListDto(
                    debtorList,
                    payment,
                    totalAmount,
                    totalPaid,
                    totalAmount - totalPaid);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<HostelDebtorListDto> CreateAsync(HostelDebtorList debtorList)
        {
            try
            {
                _context.HostelDebtorList.Add(debtorList);
                await _context.SaveChangesAsync();

                var payment = await _context.ApplicantPayment
                    .Include(ap => ap.ApplicantPayDetails)
                        .ThenInclude(apd => apd.ApplicantPayCategory)
                    .Where(ap => ap.ApplicationFormId == debtorList.ApplicationFormId)
                    .OrderByDescending(ap => ap.Id)
                    .FirstOrDefaultAsync();

                if (payment == null)
                    return null;

                decimal totalAmount = payment.ApplicantPayDetails?.ApplicantPayCategory?.TotalPayDue ?? decimal.Zero;
                decimal totalPaid = await GetTotalPaymentsByApplicationId(debtorList.ApplicationFormId);

                return CreateDebtorListDto(
                    debtorList,
                    payment,
                    totalAmount,
                    totalPaid,
                    totalAmount - totalPaid);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> CheckPaymentExistsAsync(long applicationFormId)
        {
            try
            {
                return await _context.ApplicantPayment
                    .AnyAsync(p => p.ApplicationFormId == applicationFormId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking payment existence: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsValidPaymentCategory(long applicationFormId)
        {
            try
            {
                var payment = await _context.ApplicantPayment
                    .Where(p => p.ApplicationFormId == applicationFormId)
                    .FirstOrDefaultAsync();

                return payment != null 
                    && payment.ApplicantPayCategoryId >= 9 
                    && payment.ApplicantPayCategoryId <= 53;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking payment category: {ex.Message}");
                return false;
            }
        }

        public async Task<HostelDebtorListDto> UpdateAsync(HostelDebtorList debtorList)
        {
            try
            {
                var existingDebtorList = await _context.HostelDebtorList
                    .FindAsync(debtorList.Id);

                if (existingDebtorList == null)
                    return null;

                _context.Entry(existingDebtorList).CurrentValues.SetValues(debtorList);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(debtorList.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var debtorList = await _context.HostelDebtorList
                    .FindAsync(id);

                if (debtorList == null)
                    return false;

                _context.HostelDebtorList.Remove(debtorList);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}