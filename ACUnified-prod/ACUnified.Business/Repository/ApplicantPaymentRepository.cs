using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository
{
    public class ApplicantPaymentRepository : IApplicantPaymentRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        private readonly IHostelDebtorListRepository _hostelDebtorListRepository;
        public ApplicantPaymentRepository(
        DbContextOptions<ApplicationDbContext> options,
        IMapper mapper,
        IHostelDebtorListRepository hostelDebtorListRepository) // Inject HostelDebtorListRepository
        {
            _mapper = mapper;
            dbOptions = options;
            _hostelDebtorListRepository = hostelDebtorListRepository;
        }

        public async Task<ApplicantPaymentDto> CreateApplicantPayment(ApplicantPaymentDto ApplicantPaymentDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicantPayment ApplicantPayment = _mapper.Map<ApplicantPaymentDto, ApplicantPayment>(ApplicantPaymentDto);
                var addedApplicantPayment = db.ApplicantPayment.Add(ApplicantPayment);
                await db.SaveChangesAsync();
                return _mapper.Map<ApplicantPayment, ApplicantPaymentDto>(addedApplicantPayment.Entity);
            }
        }
        public async Task<ApplicantPaymentDto> CreateApplicantPayments(ApplicantPaymentDto paymentDto)
        {
            using var db = new ApplicationDbContext(dbOptions);
            var payment = _mapper.Map<ApplicantPayment>(paymentDto);

            db.ApplicantPayment.Add(payment);
            await db.SaveChangesAsync();

            var createdDto = _mapper.Map<ApplicantPaymentDto>(payment);

            try
            {
                // Only trigger HostelDebtor creation if successful and pay category is hostel
                if (payment.isSuccessful && payment.ApplicantPayCategoryId == 60 && payment.ApplicationFormId.HasValue)
                {
                    await _hostelDebtorListRepository.GetByApplicationFormIdAsync(payment.ApplicationFormId.Value);
                    Console.WriteLine("HostelDebtorList updated automatically after payment.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create/update HostelDebtorList: {ex.Message}");
            }

            return createdDto;
        }

        public async Task<bool> CreatePayments(List<ApplicantPaymentDto> ApplicantPaymentDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                List<ApplicantPayment> ApplicantPayment = _mapper.Map<List<ApplicantPaymentDto>, List<ApplicantPayment>>(ApplicantPaymentDto);
                var addedApplicantPayment = db.ApplicantPayment.AddRangeAsync(ApplicantPayment);
                await db.SaveChangesAsync();
                return addedApplicantPayment.IsCompletedSuccessfully;
            }
        }
  public async Task<IEnumerable<ApplicationFormDto>> GetAll()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var applicationForms = await db.ApplicationForm
                    .Include(x => x.ApplicationUser)
                    .ToListAsync();
                
                return _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(applicationForms);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAll: {ex.Message}");
            return new List<ApplicationFormDto>();
        }
    }
        public async Task<IEnumerable<ApplicantPaymentDto>> GetAllPayment()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPaymentDto> ApplicantPaymentDtos =
                        _mapper.Map<IEnumerable<ApplicantPayment>, IEnumerable<ApplicantPaymentDto>>(
                            db.ApplicantPayment.Include(x => x.ApplicantPayDetails)
                            .Include(x => x.ApplicantPayDetails.ApplicantPayCategory)
                            .Include(x => x.ApplicationForm)
                            .ThenInclude(p => p.BioData)
                             .Include(x => x.ApplicationForm)
                            .ThenInclude(p => p.ProgramSetup)

                            .Include(x => x.Semester)
                            .Include(x => x.Semester.Session)
                            );
                    //var ApplicantPaymentNames = ApplicantPaymentDtos.Select(x => x.tran_ref).ToList();
                    //db.ApplicantPayment
                    //    .Include(x => x.ApplicantPayDetails)
                    //    .Where(x => ApplicantPaymentNames.Contains(x.tran_ref));
                    return ApplicantPaymentDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicantPaymentDto>> GetAllPaymentByUserId(string UserId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPaymentDto> ApplicantPaymentDtos =
                        _mapper.Map<IEnumerable<ApplicantPayment>, IEnumerable<ApplicantPaymentDto>>(db.ApplicantPayment.Include(x=>x.ApplicantPayCategory).Where(x=>x.UserId==UserId && x.isSuccessful == true));
                    //var ApplicantPaymentNames = ApplicantPaymentDtos.Select(x => x.tran_ref).ToList();
                    //var result=db.ApplicantPayment.Where(x =>  x.UserId == UserId);
                    return ApplicantPaymentDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

public async Task<IEnumerable<ApplicantPaymentDto>> GetAllPaymentByReferenceNo(string referenceNo)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Console.WriteLine($"Searching for payments with referenceNo: {referenceNo}");

            var query = db.ApplicantPayment
                .Include(x => x.ApplicantPayDetails)
                .Include(x => x.ApplicantPayCategory)
                .Include(x => x.ApplicantPayDetails.ApplicantPayCategory)
                   .Include(x=> x.ApplicationForm)
                .Include(x => x.Semester)
                .Where(x => x.ReferenceNo == referenceNo);

            Console.WriteLine($"SQL Query: {query.ToQueryString()}");

            var applicantPayments = await query.ToListAsync();

            Console.WriteLine($"Found {applicantPayments.Count} payments for reference {referenceNo}");

            if (applicantPayments.Count == 0)
            {
                // If no payments found, let's check if the referenceNo exists at all
                var anyPaymentWithRef = await db.ApplicantPayment.AnyAsync(x => x.ReferenceNo == referenceNo);
                Console.WriteLine($"Any payment with this referenceNo exists: {anyPaymentWithRef}");
            }

            var applicantPaymentDtos = _mapper.Map<IEnumerable<ApplicantPayment>, IEnumerable<ApplicantPaymentDto>>(applicantPayments);

            Console.WriteLine($"Mapped {applicantPaymentDtos.Count()} DTOs");

            return applicantPaymentDtos;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GetAllPaymentByReferenceNo: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw;
    }
}
      public async Task<ApplicantPaymentDto> GetAllPaymentReferenceNo(string referenceNo)
{
    try
    {
        Console.WriteLine($"Searching for reference no: {referenceNo}");
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var data = await db.ApplicantPayment
                .Where(x => x.ReferenceNo == referenceNo)
                .Include(x => x.ApplicantPayDetails)
                .Include(x => x.ApplicantPayCategory)
                .Include(x => x.Semester)
                .ThenInclude(s => s.Session)
                .FirstOrDefaultAsync();

            Console.WriteLine($"Data found: {data != null}");

            if (data != null)
            {
                ApplicantPaymentDto applicantPaymentDto = _mapper.Map<ApplicantPayment, ApplicantPaymentDto>(data);
                Console.WriteLine($"DTO created: {applicantPaymentDto != null}");
                return applicantPaymentDto;
            }
            else
            {
                Console.WriteLine($"No data found for reference no: {referenceNo}");
                return null;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception in GetAllPaymentReferenceNo: {ex.Message}");
        return null;
    }
}
        public async Task<bool>PaymentExists(ApplicantPaymentDto ApplicantPaymentDto)
        {
            if (ApplicantPaymentDto == null) return false;
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {

                    return await db.ApplicantPayment.AnyAsync(f => f.tran_ref == ApplicantPaymentDto.tran_ref);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

           public async Task<bool> UpdatePayment(ApplicantPaymentDto paymentDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var payment = await db.ApplicantPayment.FirstOrDefaultAsync(x => x.Id == paymentDto.Id);
                if (payment == null)
                {
                    return false;
                }

                // Update ApplicationFormId and other properties
                payment.ApplicationFormId = paymentDto.ApplicationFormId;
                payment.isSuccessful = paymentDto.isSuccessful;
                payment.Comments = paymentDto.Comments;
                payment.Amount = paymentDto.Amount;
                // Add other properties you want to update

                await db.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating payment: {ex.Message}");
            return false;
        }
    }

        public async Task<ApplicantPaymentDto> GetPayment(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPaymentDto ApplicantPaymentDto =
                        _mapper.Map<ApplicantPayment, ApplicantPaymentDto>(await db.ApplicantPayment.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicantPaymentDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeletePayment(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicantPayment = db.ApplicantPayment.FirstOrDefault(x => x.Id == Id);
                if (ApplicantPayment != null)
                {
                    db.ApplicantPayment.Remove(ApplicantPayment);
                    db.SaveChanges();
                }
            }
        }

        // public async Task<ApplicantPaymentDto> UpdatePayment(ApplicantPaymentDto ApplicantPaymentDto)
        // {
        //     try
        //     {
        //         using (var db = new ApplicationDbContext(dbOptions))
        //         {
        //             ApplicantPayment ApplicantPayment = db.ApplicantPayment.FirstOrDefault(x => x.Id == ApplicantPaymentDto.Id);
        //             if (ApplicantPayment == null)
        //             {
        //                 return null;
        //             }
        //             else
        //             {
        //                 ApplicantPayment ApplicantPaymentUpdate = _mapper.Map<ApplicantPaymentDto, ApplicantPayment>(ApplicantPaymentDto, ApplicantPayment);
        //                 var currentupdate = db.ApplicantPayment.Update(ApplicantPaymentUpdate);
        //                 await db.SaveChangesAsync();
        //                 return _mapper.Map<ApplicantPayment, ApplicantPaymentDto>(currentupdate.Entity);
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return null;
        //     }
        // }


        //check if student has paid the course form fee

        //check if student have paid the mandatory fees

        public bool HasPaidMandatoryFees(string matric, long semesterId)
        {
            bool hasPaidManatoryFees = false;
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicantPayment = db.ApplicantPayment.Where(x => x.UserId == matric &&
                                                    x.SemesterId == semesterId && x.isSuccessful == true).ToList();
                if (ApplicantPayment != null)
                {
                    hasPaidManatoryFees = true;
                }

            }

            return hasPaidManatoryFees;
        }

        public bool HasPaidCourseFormFees(string matric, long semesterId)
        {
            bool hasPaidCourseFormFees = false;
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicantPayment = db.ApplicantPayment.FirstOrDefault(x => x.UserId == matric
                                                             && x.SemesterId == semesterId &&
                                                             x.ApplicantPayCategory.Name.Contains("Portal Access")
                                                             && x.isSuccessful == true);
                if (ApplicantPayment != null)
                {
                    hasPaidCourseFormFees = true;
                }

            }

            return hasPaidCourseFormFees;
        }

        public bool HasApplicantPaidFormFees(string userId, long semesterId)
        {
            bool hasApplicantPaidFees = false;
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicantPayment = db.ApplicantPayment.FirstOrDefault(x => x.UserId == userId
                                                             && x.SemesterId == semesterId &&
                                                             //applicant category is 10000
                                                             x.ApplicantPayCategoryId == 10000 && x.isSuccessful == true);
                if (ApplicantPayment != null)
                {
                    hasApplicantPaidFees = true;
                }

            }

            return hasApplicantPaidFees;
        }

       
    }
}
