using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


public class PayUploadDetailsRepository : IPayUploadDetailsRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public PayUploadDetailsRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<PayUploadDetailsDto> CreatePayUploadDetails(PayUploadDetailsDto PayUploadDetailsDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            PayUploadDetails PayUploadDetails = _mapper.Map<PayUploadDetailsDto, PayUploadDetails>(PayUploadDetailsDto);
            var addedPayment = db.PayUploadDetails.Add(PayUploadDetails);
            await db.SaveChangesAsync();
            return _mapper.Map<PayUploadDetails, PayUploadDetailsDto>(addedPayment.Entity);
        }
    }

    public async Task<IEnumerable<PayUploadDetailsDto>> GetAllPayUploadDetails()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PayUploadDetailsDto> PayUploadDetailsDtos =
                    _mapper.Map<IEnumerable<PayUploadDetails>, IEnumerable<PayUploadDetailsDto>>(db.PayUploadDetails);
                //var PayUploadDetails = PayUploadDetailsDtos.Select(x => x.Name).ToList();
                //db.PayUploadDetails.Where(x => PayUploadDetails.Contains(x.Name));
                return PayUploadDetailsDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<IEnumerable<PayUploadDetailsDto>> GetPayUploadDetailsBatch(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var data = db.PayUploadDetails.Include(x=>x.Student).Where(x => x.PayUploadCategory.
                PayUploadCategoryBatchId== Id);
                var payDetailsDto = _mapper.Map<IEnumerable<PayUploadDetails>, IEnumerable<PayUploadDetailsDto>>(data);
                return await Task.FromResult(payDetailsDto);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> PayUploadDetailsExists(PayUploadDetailsDto PayUploadDetailsDto)
    {
        if (PayUploadDetailsDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                return await db.PayUploadDetails.AnyAsync(f => f.Name == PayUploadDetailsDto.Name);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<PayUploadDetailsDto> GetPayUploadDetails(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayUploadDetailsDto PayUploadDetailsDto =
                    _mapper.Map<PayUploadDetails, PayUploadDetailsDto>(await db.PayUploadDetails.FirstOrDefaultAsync(x => x.PayUploadCategory.PayUploadCategoryBatchId == Id));
                return PayUploadDetailsDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<PayUploadDetailsDto> GetPayUploadDetailsByName(string name)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayUploadDetailsDto PayUploadDetailsDto =
                    _mapper.Map<PayUploadDetails, PayUploadDetailsDto>(await db.PayUploadDetails.FirstOrDefaultAsync(x => x.Name == name));
                return PayUploadDetailsDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
   

    public async Task DeletePayUploadDetails(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var payinfo = db.PayUploadDetails.FirstOrDefault(x => x.Id == Id);
            if (payinfo != null)
            {
                db.PayUploadDetails.Remove(payinfo);
                db.SaveChanges();
            }
        }
    }

    public async Task DeletePayUploadDetailsByCategoryBatch(long categoryBatchId)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var payinfo = db.PayUploadDetails.FirstOrDefault(x => x.PayUploadCategoryBatchId == categoryBatchId);
            if (payinfo != null)
            {
                db.PayUploadDetails.Remove(payinfo);
                db.SaveChanges();
            }
        }
    }

    public async Task<PayUploadDetailsDto> UpdatePayUploadDetails(PayUploadDetailsDto PayUploadDetailsDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                PayUploadDetails paydetaill = db.PayUploadDetails.FirstOrDefault(x => x.Id == PayUploadDetailsDto.Id);
                if (paydetaill == null)
                {
                    return null;
                }
                else
                {
                    PayUploadDetails paydetailUpdate = _mapper.Map<PayUploadDetailsDto, PayUploadDetails>(PayUploadDetailsDto, paydetaill);
                    var currentupdate = db.PayUploadDetails.Update(paydetailUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<PayUploadDetails, PayUploadDetailsDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

   


    public async Task<IEnumerable<PayUploadDetailsDto>> GetStudentPayUploadDetails(long currentSemester, long categoryId,bool isSemesterLate)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                //check the payments already paid and remove from the list
                var PayUploadDetails = await db.PayUploadDetails
                    .Include(x => x.PayUploadCategory)
                    .Where(x => x.PayUploadCategoryId == categoryId && 
                                x.PayUploadCategory.Semester.Id == 
                                currentSemester && !db.Payment.Any(p =>
                                 p.PayDetails.PayCategoryId == x.PayUploadCategoryId && p.isSuccessful==true))
                    .ToListAsync();

                // Additional query excluding already paid
                List<PayUploadDetails> additionalQuery;
                if (isSemesterLate){
                    additionalQuery = await db.PayUploadDetails
                        .Include(x => x.PayUploadCategory)
                        .Where(x => x.PayUploadCategory.Name.Contains("Portal Access Fee")
                                    && (x.PayUploadCategory.SemesterId == currentSemester) 
                                    && !db.Payment.Include(x => x.PayDetails.PayCategory)
                                        .Any(p => p.PayDetails.PayCategoryId == x.PayUploadCategoryId)
                                    
                        )
                        .ToListAsync();
                }
                else
                {
                   
                        additionalQuery = await db.PayUploadDetails
                            .Include(x => x.PayUploadCategory)
                            .Where(x => x.PayUploadCategory.Name.Contains("Portal Access Fee")
                                        && (x.PayUploadCategory.SemesterId == currentSemester)

                                        && !db.Payment.Include(x => x.PayDetails.PayCategory)
                                            .Any(p => p.PayDetails.PayCategoryId == x.PayUploadCategoryId))
                            .ToListAsync();
                    
                }

            var mergedResults = PayUploadDetails.Concat(additionalQuery)
                    .GroupBy(x => x.Id) // Assuming 'Id' is a unique identifier for PayUploadDetails
                    .Select(group => group.First())
                    .ToList();
                
                if (!mergedResults.Any())
                {
                    // Optionally handle the case where no matching pay categories are found
                    return Enumerable.Empty<PayUploadDetailsDto>();
                }

                var payCategoryDtos = _mapper.Map<IEnumerable<PayUploadDetails>, IEnumerable<PayUploadDetailsDto>>(mergedResults);
                return payCategoryDtos;
            }
        }
        catch (Exception ex)
        {
            // Log the exception here for debugging purposes
            return Enumerable.Empty<PayUploadDetailsDto>();
        }
    }

    public Task<IEnumerable<PayUploadDetailsDto>> GetStudentPayUploadDetails(SemesterType currentSemester, long categoryId, bool isSemesterLate)
    {
        throw new NotImplementedException();
    }

    public async Task CreatePayUploadDetails(IEnumerable<PayUploadDetailsDto> payUploadDetailsDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<PayUploadDetails> PayUploadDetails = _mapper.Map<IEnumerable<PayUploadDetailsDto>, IEnumerable<PayUploadDetails>>(payUploadDetailsDto);
                db.PayUploadDetails.AddRange(PayUploadDetails);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {

            await Console.Out.WriteLineAsync("error at "+e);
        }
        
    }
}
