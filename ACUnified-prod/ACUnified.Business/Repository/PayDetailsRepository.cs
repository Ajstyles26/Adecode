using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;
using System.Data.Entity;


public class PayDetailsRepository : IPayDetailsRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public PayDetailsRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<PayDetailsDto> CreatePayDetails(PayDetailsDto PayDetailsDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            PayDetails paydetails = _mapper.Map<PayDetailsDto, PayDetails>(PayDetailsDto);
            var addedPayment = db.PayDetails.Add(paydetails);
            await db.SaveChangesAsync();
            return _mapper.Map<PayDetails, PayDetailsDto>(addedPayment.Entity);
        }
    }

    public async Task CreatePayDetails(IEnumerable<PayDetailsDto> PayDetailsDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
           
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    IEnumerable<PayDetails> paydetails = _mapper.Map<IEnumerable<PayDetailsDto>, IEnumerable<PayDetails>>(PayDetailsDto);
                    db.PayDetails.AddRange(paydetails);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Handle exceptions (e.g., duplicate key violation) gracefully
                }
            }

        }
    }

    public async Task<IEnumerable<PayDetailsDto>> GetAllPayDetails()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PayDetailsDto> PayDetailsDtos =
                    _mapper.Map<IEnumerable<PayDetails>, IEnumerable<PayDetailsDto>>(db.PayDetails);
                //var payDetails = PayDetailsDtos.Select(x => x.Name).ToList();
                //db.PayDetails.Where(x => payDetails.Contains(x.Name));
                return PayDetailsDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> PayDetailsExists(PayDetailsDto PayDetailsDto)
    {
        if (PayDetailsDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                return await db.PayDetails.AnyAsync(f => f.Name == PayDetailsDto.Name);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<PayDetailsDto> GetPayDetails(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayDetailsDto PayDetailsDto =
                    _mapper.Map<PayDetails, PayDetailsDto>(await db.PayDetails.FirstOrDefaultAsync(x => x.Id == Id));
                return PayDetailsDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

   
    public async Task<PayDetailsDto> GetPayDetailsByName(string name)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayDetailsDto PayDetailsDto =
                    _mapper.Map<PayDetails, PayDetailsDto>(await db.PayDetails.FirstOrDefaultAsync(x => x.Name == name));
                return PayDetailsDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
   

    public async Task DeletePayDetails(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var payinfo = db.PayDetails.FirstOrDefault(x => x.Id == Id);
            if (payinfo != null)
            {
                db.PayDetails.Remove(payinfo);
                db.SaveChanges();
            }
        }
    }

    public async Task<PayDetailsDto> UpdatePayDetails(PayDetailsDto PayDetailsDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayDetails paydetaill = db.PayDetails.FirstOrDefault(x => x.Id == PayDetailsDto.Id);
                if (paydetaill == null)
                {
                    return null;
                }
                else
                {
                    PayDetails paydetailUpdate = _mapper.Map<PayDetailsDto, PayDetails>(PayDetailsDto, paydetaill);
                    var currentupdate = db.PayDetails.Update(paydetailUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<PayDetails, PayDetailsDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

   


    public async Task<IEnumerable<PayDetailsDto>> GetStudentPayDetails(long? curentSession,long currentSemester, long studentId,bool isSemesterLate)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                //check the payments already paid and remove from the list
                var payDetail = db.PayDetails.

                    Where(x => x.StudentId == studentId && x.SessionId == curentSession).AsEnumerable();

                //var payDetailsAndPayments = db.PayDetails
                //.GroupJoin(db.Payment,
                //    pd => pd.Id,
                //    p => p.PayDetailsId,
                //    (pd, payments) => new { PayDetails = pd, Payments = payments })
                //    .Where(x => x.PayDetails.StudentId == studentId && x.PayDetails.SessionId == curentSession)
                //    .SelectMany(x => x.Payments.DefaultIfEmpty(),
                //    (pd, payment) => new { PayDetails = pd.PayDetails, Payment = payment });

                //IEnumerable<PayDetails> pdetail= payDetail.Where(x => x.PayDetail!=null).Select(x=>x.PayDetails);
                var payCategoryDtos = _mapper.Map<IEnumerable<PayDetails>, IEnumerable<PayDetailsDto>>(payDetail);
                return payCategoryDtos;
            }
        }
        catch (Exception)
        {

            return Enumerable.Empty<PayDetailsDto>();
        }
        //try
        //{
        //    using (var db = new ApplicationDbContext(dbOptions))
        //    {
        //        //check the payments already paid and remove from the list
        //        var payDetails = await db.PayDetails
        //            .Include(x => x.PayCategory)
        //            .Where(x => x.PayCategoryId == categoryId && 
        //                        x.PayCategory.Semester.Id == 
        //                        currentSemester && !db.Payment.Any(p =>
        //                         p.PaySetupDetails.PayCategoryId == x.PayCategoryId && p.isSuccessful==true))
        //            .ToListAsync();

        //        // Additional query excluding already paid
        //        List<PayDetails> additionalQuery;
        //        if (isSemesterLate){
        //            additionalQuery = await db.PayDetails
        //                .Include(x => x.PayCategory)
        //                .Where(x => x.PayCategory.Name.Contains("Portal Access Fee")
        //                            && (x.PayCategory.SemesterId == currentSemester) 
        //                            && !db.Payment.Include(x => x.PaySetupDetails.PayCategory)
        //                                .Any(p => p.PaySetupDetails.PayCategoryId == x.PayCategoryId )
                                    
        //                )
        //                .ToListAsync();
        //        } 
        //        else
        //        {
                   
        //                additionalQuery = await db.PayDetails
        //                    .Include(x => x.PayCategory)
        //                    .Where(x => x.PayCategory.Name.Contains("Portal Access Fee")
        //                                && (x.PayCategory.SemesterId == currentSemester)

        //                                && !db.Payment.Include(x => x.PaySetupDetails.PayCategory)
        //                                    .Any(p => p.PaySetupDetails.PayCategoryId == x.PayCategoryId))
        //                    .ToListAsync();
                    
        //        }

        //    var mergedResults = payDetails.Concat(additionalQuery)
        //            .GroupBy(x => x.Id) // Assuming 'Id' is a unique identifier for PayDetails
        //            .Select(group => group.First())
        //            .ToList();
                
        //        if (!mergedResults.Any())
        //        {
        //            // Optionally handle the case where no matching pay categories are found
        //            return Enumerable.Empty<PayDetailsDto>();
        //        }

        //        var payCategoryDtos = _mapper.Map<IEnumerable<PayDetails>, IEnumerable<PayDetailsDto>>(mergedResults);
        //        return payCategoryDtos;
        //    }
        //}
        //catch (Exception ex)
        //{
            // Log the exception here for debugging purposes
          //  return Enumerable.Empty<PayDetailsDto>();
        //}
    }

    public Task<IEnumerable<PayDetailsDto>> GetStudentPayDetails(SemesterType currentSemester, long categoryId, bool isSemesterLate)
    {
        throw new NotImplementedException();
    }
}
