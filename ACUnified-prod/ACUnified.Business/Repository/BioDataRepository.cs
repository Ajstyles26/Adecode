using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Data.Models;

using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository
{
    public class BioDataRepository : IBioDataRepository
    {
      
        private readonly IMapper _mapper;
        private readonly IAcademicSessionRepository _academicSessionRepository;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        public BioDataRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper,
            IAcademicSessionRepository academicSessionRepository)
        {
            _mapper = mapper;
            _academicSessionRepository = academicSessionRepository;
            dbOptions = options;
        }
    
        public async Task<BioDataDto> CreateBioData(BioDataDto biodataDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var currentSession=await _academicSessionRepository.GetActiveApplicantSession();
                //get active session for applicant
                if (currentSession.Count() > 0)
                {
                    biodataDto.SessionId = currentSession.FirstOrDefault().Id;
                }
                else
                {
                    biodataDto.SessionId = 14;
                }
               
                //pick an arbitrary data source
                BioData biodata = _mapper.Map<BioDataDto, BioData>(biodataDto);
                var addedBioData = db.BioData.Add(biodata);
                await db.SaveChangesAsync();
                return _mapper.Map<BioData, BioDataDto>(addedBioData.Entity);
            }
        }
    
        public async Task<IEnumerable<BioDataDto>> GetAllBioData()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<BioDataDto> biodataDtos =
                        _mapper.Map<IEnumerable<BioData>, IEnumerable<BioDataDto>>(db.BioData);
                    return biodataDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    
        public async Task<BioDataDto> GetBioData(string userid)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    BioDataDto biodataDto =
                        _mapper.Map<BioData, BioDataDto>(await db.BioData.FirstOrDefaultAsync(x => x.UserId == userid));
                    if (biodataDto == null) { return null; }
                    return biodataDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteBioData(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var biodata = db.BioData.FirstOrDefault(x => x.Id == Id);
                if (biodata != null)
                {
                    db.BioData.Remove(biodata);
                    db.SaveChanges();
                }
            }
        }

        // public async Task<BioDataDto> UpdateBioData(BioDataDto biodataDto)
        // {
        //     try
        //     {
        //         using (var db = new ApplicationDbContext(dbOptions))
        //         {
        //             BioData biodata =db.BioData.FirstOrDefault(x => x.Id == biodataDto.Id);
        //             if (biodata == null)
        //             {
        //                 return null;
        //             }
        //             else
        //             {
        //                 BioData biodataUpdate = _mapper.Map<BioDataDto, BioData>(biodataDto, biodata);
        //                 var currentupdate = db.BioData.Update(biodataUpdate);
        //                 await db.SaveChangesAsync();
        //                 return _mapper.Map<BioData, BioDataDto>(currentupdate.Entity);
        //             }
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return null;
        //     }
        // }
        public async Task<BioDataDto> UpdateBioData(BioDataDto biodataDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var biodata = await db.BioData.FirstOrDefaultAsync(x => x.UserId == biodataDto.UserId);
                    if (biodata == null)
                    {
                        // If the biodata doesn't exist, create a new one
                        return await CreateBioData(biodataDto);
                    }
                    else
                    {
                        // Update the existing biodata
                        _mapper.Map(biodataDto, biodata);
                        db.BioData.Update(biodata);
                        await db.SaveChangesAsync();
                        return _mapper.Map<BioDataDto>(biodata);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating biodata for user {biodataDto.UserId}: {ex.Message}");
                return null;
            }
        }
       
}
    

}
