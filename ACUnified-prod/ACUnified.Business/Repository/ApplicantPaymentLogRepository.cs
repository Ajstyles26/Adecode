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
    public class ApplicantPaymentLogRepository : IApplicantPaymentLogRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public ApplicantPaymentLogRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<ApplicantPaymentLogDto> CreateApplicantPayment(ApplicantPaymentLogDto ApplicantPaymentLogDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicantPaymentLog ApplicantPaymentLog = _mapper.Map<ApplicantPaymentLogDto, ApplicantPaymentLog>(ApplicantPaymentLogDto);
                var addedApplicantPayment = db.ApplicantPaymentLog.Add(ApplicantPaymentLog);
                await db.SaveChangesAsync();
                return _mapper.Map<ApplicantPaymentLog, ApplicantPaymentLogDto>(addedApplicantPayment.Entity);
            }
        }

        public async Task<bool> CreatePayments(List<ApplicantPaymentLogDto> ApplicantPaymentLogDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                List<ApplicantPaymentLog> ApplicantPaymentLog = _mapper.Map<List<ApplicantPaymentLogDto>, List<ApplicantPaymentLog>>(ApplicantPaymentLogDto);
                var addedApplicantPayment = db.ApplicantPaymentLog.AddRangeAsync(ApplicantPaymentLog);
                await db.SaveChangesAsync();
                return addedApplicantPayment.IsCompletedSuccessfully;
            }
        }

        public async Task<IEnumerable<ApplicantPaymentLogDto>> GetAllPayment()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPaymentLogDto> ApplicantPaymentLogDtos =
                        _mapper.Map<IEnumerable<ApplicantPaymentLog>, IEnumerable<ApplicantPaymentLogDto>>(
                            db.ApplicantPaymentLog.Include(x => x.ApplicantPayDetails)
                            .Include(x => x.ApplicantPayDetails.ApplicantPayCategory)
                            .Include(x => x.Semester)
                            .Include(x => x.Semester.Session)
                            );
                    //var ApplicantPaymentNames = ApplicantPaymentLogDtos.Select(x => x.tran_ref).ToList();
                    //db.ApplicantPaymentLog
                    //    .Include(x => x.ApplicantPayDetails)
                    //    .Where(x => ApplicantPaymentNames.Contains(x.tran_ref));
                    return ApplicantPaymentLogDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ApplicantPaymentLogDto> GetLastPaymentByUserId(string userId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    // Get the most recent payment for the user
                    var lastPayment = await db.ApplicantPaymentLog
                        .Include(p => p.ApplicantPayCategory)
                        .Include(p => p.ApplicantPayDetails)
                        .Include(p => p.Semester)
                        // .Include(p => p.AcademicSession)
                        // .Include(p => p.ProgramSetup)
                        .Where(p => p.UserId == userId)
                        .OrderByDescending(p => p.CreatedDate)
                        .FirstOrDefaultAsync();

                    // Map to DTO and return
                    return _mapper.Map<ApplicantPaymentLog, ApplicantPaymentLogDto>(lastPayment);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving last payment: {ex.Message}", ex);
            }
        }

        public async Task UpdateApplicantPayment(ApplicantPaymentLogDto paymentDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    // Get existing entity from database
                    var existingPayment = await db.ApplicantPaymentLog
                        .FirstOrDefaultAsync(p => p.Id == paymentDto.Id);

                    if (existingPayment == null)
                    {
                        throw new Exception($"Payment with ID {paymentDto.Id} not found");
                    }

                    // Update fields
                    existingPayment.Amount = paymentDto.Amount;
                    existingPayment.Comments = paymentDto.Comments;
                    existingPayment.ReferenceNo = paymentDto.ReferenceNo;
                    existingPayment.RRRRNo = paymentDto.RRRRNo;
                    existingPayment.isSuccessful = paymentDto.isSuccessful;
                    existingPayment.tran_status = paymentDto.tran_status;
                    existingPayment.pay_response = paymentDto.pay_response;

                    // Save changes
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating payment: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ApplicantPaymentLogDto>> GetAllPaymentByUserId(string UserId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPaymentLogDto> ApplicantPaymentLogDtos =
                        _mapper.Map<IEnumerable<ApplicantPaymentLog>, IEnumerable<ApplicantPaymentLogDto>>(db.ApplicantPaymentLog.Include(x=>x.ApplicantPayCategory).Where(x=>x.UserId==UserId && x.isSuccessful == true));
                    //var ApplicantPaymentNames = ApplicantPaymentLogDtos.Select(x => x.tran_ref).ToList();
                    //var result=db.ApplicantPaymentLog.Where(x =>  x.UserId == UserId);
                    return ApplicantPaymentLogDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicantPaymentLogDto>> GetAllPaymentByEmail(string Email)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPaymentLogDto> ApplicantPaymentLogDtos =
                        _mapper.Map<IEnumerable<ApplicantPaymentLog>, IEnumerable<ApplicantPaymentLogDto>>(db.ApplicantPaymentLog.Include(x => x.ApplicantPayDetails).
                            Include(x => x.ApplicantPayCategory).
                            Include(x => x.ApplicantPayDetails.ApplicantPayCategory).
                            Include(x => x.Semester).
                            Include(x => x.Semester.Session).
                            Where(x => x.email == Email).ToList()
                        );
                    //var ApplicantPaymentNames = ApplicantPaymentLogDtos.Select(x => x.tran_ref).ToList();
                    //var result=db.ApplicantPaymentLog.Where(x =>  x.UserId == UserId);
                    return ApplicantPaymentLogDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<IEnumerable<ApplicantPaymentLogDto>> GetAllPaymentByReferenceNo(string referenceNo)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPaymentLogDto> ApplicantPaymentLogDtos =
                        _mapper.Map<IEnumerable<ApplicantPaymentLog>, IEnumerable<ApplicantPaymentLogDto>>
                        (db.ApplicantPaymentLog.Include(x => x.ApplicantPayDetails).
                            Include(x=>x.ApplicantPayCategory).
                            Include(x => x.ApplicantPayDetails.ApplicantPayCategory).
                            Include(x => x.Semester).
                            Include(x => x.Semester.Session).
                            Where(x => x.ReferenceNo == referenceNo).ToList()
                        );
                    //var ApplicantPaymentNames = ApplicantPaymentLogDtos.Select(x => x.ReferenceNo).ToList();
                    //db.ApplicantPaymentLog.Where(x => ApplicantPaymentNames.Contains(x.ReferenceNo) && x.ReferenceNo == referenceNo);
                    return ApplicantPaymentLogDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool>PaymentExists(ApplicantPaymentLogDto ApplicantPaymentLogDto)
        {
            if (ApplicantPaymentLogDto == null) return false;
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {

                    return await db.ApplicantPaymentLog.AnyAsync(f => f.tran_ref == ApplicantPaymentLogDto.tran_ref);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<ApplicantPaymentLogDto> GetPayment(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPaymentLogDto ApplicantPaymentLogDto =
                        _mapper.Map<ApplicantPaymentLog, ApplicantPaymentLogDto>(await db.ApplicantPaymentLog.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicantPaymentLogDto;
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
                var ApplicantPaymentLog = db.ApplicantPaymentLog.FirstOrDefault(x => x.Id == Id);
                if (ApplicantPaymentLog != null)
                {
                    db.ApplicantPaymentLog.Remove(ApplicantPaymentLog);
                    db.SaveChanges();
                }
            }
        }

        public async Task<ApplicantPaymentLogDto> UpdatePayment(ApplicantPaymentLogDto ApplicantPaymentLogDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPaymentLog ApplicantPaymentLog = db.ApplicantPaymentLog.FirstOrDefault(x => x.Id == ApplicantPaymentLogDto.Id);
                    if (ApplicantPaymentLog == null)
                    {
                        return null;
                    }
                    else
                    {
                        ApplicantPaymentLog ApplicantPaymentUpdate = _mapper.Map<ApplicantPaymentLogDto, ApplicantPaymentLog>(ApplicantPaymentLogDto, ApplicantPaymentLog);
                        var currentupdate = db.ApplicantPaymentLog.Update(ApplicantPaymentUpdate);
                        await db.SaveChangesAsync();
                        return _mapper.Map<ApplicantPaymentLog, ApplicantPaymentLogDto>(currentupdate.Entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //check if student has paid the course form fee

        //check if student have paid the mandatory fees

        public bool HasPaidMandatoryFees(string matric, long semesterId)
        {
            bool hasPaidManatoryFees = false;
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicantPaymentLog = db.ApplicantPaymentLog.Where(x => x.UserId == matric &&
                                                    x.SemesterId == semesterId && x.isSuccessful == true).ToList();
                if (ApplicantPaymentLog != null)
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
                var ApplicantPaymentLog = db.ApplicantPaymentLog.FirstOrDefault(x => x.UserId == matric
                                                             && x.SemesterId == semesterId &&
                                                             x.ApplicantPayCategory.Name.Contains("Portal Access")
                                                             && x.isSuccessful == true);
                if (ApplicantPaymentLog != null)
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
                var ApplicantPaymentLog = db.ApplicantPaymentLog.FirstOrDefault(x => x.UserId == userId
                                                             && x.SemesterId == semesterId &&
                                                             //applicant category is 10000
                                                             x.ApplicantPayCategoryId == 10000 && x.isSuccessful == true);
                if (ApplicantPaymentLog != null)
                {
                    hasApplicantPaidFees = true;
                }

            }

            return hasApplicantPaidFees;
        }

       
    }
}
