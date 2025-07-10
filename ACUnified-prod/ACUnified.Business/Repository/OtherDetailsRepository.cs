using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


namespace ACUnified.Business.Repository
{
    public class OtherDetailsRepository : IOtherDetailsRepository
    {
      
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        public OtherDetailsRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }
    
        public async Task<OtherDetailsDto> CreateOtherDetails(OtherDetailsDto otherdetailsDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                OtherDetails otherdetails = _mapper.Map<OtherDetailsDto, OtherDetails>(otherdetailsDto);
                var addedOtherDetails = db.OtherDetails.Add(otherdetails);
                await db.SaveChangesAsync();
                return _mapper.Map<OtherDetails, OtherDetailsDto>(addedOtherDetails.Entity);
            }
        }
    
        public async Task<IEnumerable<OtherDetailsDto>> GetAllOtherDetails()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<OtherDetailsDto> otherdetailsDtos =
                        _mapper.Map<IEnumerable<OtherDetails>, IEnumerable<OtherDetailsDto>>(db.OtherDetails);
                    return otherdetailsDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OtherDetailsDto> GetOtherDetails(string userid)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    OtherDetailsDto otherdetailsDto =
                        _mapper.Map<OtherDetails, OtherDetailsDto>(await db.OtherDetails.FirstOrDefaultAsync(x => x.UserId == userid));
                    //if (otherdetailsDto == null) { return null; }
                    return otherdetailsDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteOtherDetails(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var otherdetails = db.OtherDetails.FirstOrDefault(x => x.Id == Id);
                if (otherdetails != null)
                {
                    db.OtherDetails.Remove(otherdetails);
                    db.SaveChanges();
                }
            }
        }

        // public async Task<OtherDetailsDto> UpdateOtherDetails(OtherDetailsDto otherdetailsDto)
        // {
        //     try
        //     {
        //         using (var db = new ApplicationDbContext(dbOptions))
        //         {
        //             OtherDetails otherdetails =db.OtherDetails.FirstOrDefault(x => x.Id == otherdetailsDto.Id);
        //             if (otherdetails == null)
        //             {
        //                 return null;
        //             }
        //             else
        //             {
        //                 OtherDetails otherdetailsUpdate = _mapper.Map<OtherDetailsDto, OtherDetails>(otherdetailsDto, otherdetails);
        //                 var currentupdate = db.OtherDetails.Update(otherdetailsUpdate);
        //                 await db.SaveChangesAsync();
        //                 return _mapper.Map<OtherDetails, OtherDetailsDto>(currentupdate.Entity);
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return null;
        //     }
        // }

        public async Task<OtherDetailsDto> UpdateOtherDetails(OtherDetailsDto otherDetailsDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var otherDetails = await db.OtherDetails.FirstOrDefaultAsync(x => x.UserId == otherDetailsDto.UserId);
                    if (otherDetails == null)
                    {
                        // If the other details don't exist, create a new one
                        return await CreateOtherDetails(otherDetailsDto);
                    }
                    else
                    {
                        // Update the existing other details
                        _mapper.Map(otherDetailsDto, otherDetails);
                        db.OtherDetails.Update(otherDetails);
                        await db.SaveChangesAsync();
                        return _mapper.Map<OtherDetailsDto>(otherDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating other details for user {otherDetailsDto.UserId}: {ex.Message}");
                return null;
            }
        }
    }

}
