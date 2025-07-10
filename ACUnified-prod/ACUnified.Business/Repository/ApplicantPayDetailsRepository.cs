using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Business.Repository
{



    public class ApplicantPayDetailsRepository : IApplicantPayDetailsRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public ApplicantPayDetailsRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<ApplicantPayDetailsDto> CreatePayDetails(ApplicantPayDetailsDto ApplicantPayDetailsDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicantPayDetails ApplicantPayDetails = _mapper.Map<ApplicantPayDetailsDto, ApplicantPayDetails>(ApplicantPayDetailsDto);
                var addedPayment = db.ApplicantPayDetails.Add(ApplicantPayDetails);
                await db.SaveChangesAsync();
                return _mapper.Map<ApplicantPayDetails, ApplicantPayDetailsDto>(addedPayment.Entity);
            }
        }

        public async Task<IEnumerable<ApplicantPayDetailsDto>> GetAllPayDetails()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicantPayDetailsDto> ApplicantPayDetailsDtos =
                        _mapper.Map<IEnumerable<ApplicantPayDetails>, IEnumerable<ApplicantPayDetailsDto>>(db.ApplicantPayDetails);
                    //var ApplicantPayDetails = ApplicantPayDetailsDtos.Select(x => x.Name).ToList();
                    //db.ApplicantPayDetails.Where(x => ApplicantPayDetails.Contains(x.Name));
                    return ApplicantPayDetailsDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Add this new method
        public async Task<IEnumerable<ApplicantPayDetailsDto>> GetPayDetailsByProgramSetupId(long programSetupId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var payDetails = await db.ApplicantPayDetails
                        .Include(x => x.ApplicantPayCategory)
                        .Include(x => x.ProgramSetup)
                        .Include(x => x.Session)
                        .Where(x => x.ProgramSetupId == programSetupId)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<ApplicantPayDetails>, IEnumerable<ApplicantPayDetailsDto>>(payDetails);
                }
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ApplicantPayDetailsDto>();
            }
        }

        public async Task<bool> ApplicantPayDetailsExists(ApplicantPayDetailsDto ApplicantPayDetailsDto)
        {
            if (ApplicantPayDetailsDto == null) return false;
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    return await db.ApplicantPayDetails.AnyAsync(f => f.Name == ApplicantPayDetailsDto.Name);
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<ApplicantPayDetailsDto> GetPayDetails(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPayDetailsDto ApplicantPayDetailsDto =
                        _mapper.Map<ApplicantPayDetails, ApplicantPayDetailsDto>(await db.ApplicantPayDetails.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicantPayDetailsDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ApplicantPayDetailsDto> GetPayDetailsByName(string name)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPayDetailsDto ApplicantPayDetailsDto =
                        _mapper.Map<ApplicantPayDetails, ApplicantPayDetailsDto>(await db.ApplicantPayDetails.FirstOrDefaultAsync(x => x.Name == name));
                    return ApplicantPayDetailsDto;
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
                var payinfo = db.ApplicantPayDetails.FirstOrDefault(x => x.Id == Id);
                if (payinfo != null)
                {
                    db.ApplicantPayDetails.Remove(payinfo);
                    db.SaveChanges();
                }
            }
        }

        public async Task<ApplicantPayDetailsDto> UpdatePayDetails(ApplicantPayDetailsDto ApplicantPayDetailsDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicantPayDetails paydetaill = db.ApplicantPayDetails.FirstOrDefault(x => x.Id == ApplicantPayDetailsDto.Id);
                    if (paydetaill == null)
                    {
                        return null;
                    }
                    else
                    {
                        ApplicantPayDetails paydetailUpdate = _mapper.Map<ApplicantPayDetailsDto, ApplicantPayDetails>(ApplicantPayDetailsDto, paydetaill);
                        var currentupdate = db.ApplicantPayDetails.Update(paydetailUpdate);
                        await db.SaveChangesAsync();
                        return _mapper.Map<ApplicantPayDetails, ApplicantPayDetailsDto>(currentupdate.Entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public async Task<IEnumerable<ApplicantPayDetailsDto>> GetStudentPayDetails(SemesterType currentSemester, long categoryId, bool isSemesterLate)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    //check the payments already paid and remove from the list
                    var ApplicantPayDetails = await db.ApplicantPayDetails
                        .Include(x => x.ApplicantPayCategory)
                        .Where(x => x.ApplicantPayCategoryId == categoryId &&
                                    x.Semester ==
                                    currentSemester && !db.Payment.Any(p =>
                                      p.isSuccessful == true))
                        .ToListAsync();

                    // Additional query excluding already paid
                    List<ApplicantPayDetails> additionalQuery;
                    if (isSemesterLate)
                    {
                        additionalQuery = await db.ApplicantPayDetails
                            .Include(x => x.ApplicantPayCategory)
                            .Where(x => x.ApplicantPayCategory.Name.Contains("Portal Access Fee")
                                        && (x.Semester == currentSemester)
                                        )

                            .ToListAsync();
                    }
                    else
                    {

                        additionalQuery = await db.ApplicantPayDetails
                            .Include(x => x.ApplicantPayCategory)
                            .Where(x => x.ApplicantPayCategory.Name.Contains("Portal Access Fee")
                                        && (x.Semester == currentSemester))
                            .ToListAsync();

                    }

                    var mergedResults = ApplicantPayDetails.Concat(additionalQuery)
                            .GroupBy(x => x.Id) // Assuming 'Id' is a unique identifier for ApplicantPayDetails
                            .Select(group => group.First())
                            .ToList();

                    if (!mergedResults.Any())
                    {
                        // Optionally handle the case where no matching pay categories are found
                        return Enumerable.Empty<ApplicantPayDetailsDto>();
                    }

                    var payCategoryDtos = _mapper.Map<IEnumerable<ApplicantPayDetails>, IEnumerable<ApplicantPayDetailsDto>>(mergedResults);
                    return payCategoryDtos;
                }
            }
            catch (Exception ex)
            {
                // Log the exception here for debugging purposes
                return Enumerable.Empty<ApplicantPayDetailsDto>();
            }
        }

      
    }

}
