using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


namespace ACUnified.Business.Repository
{
    public class NextOfKinRepository : INextOfKinRepository
    {
      
        private readonly IMapper _mapper;
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        public NextOfKinRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }
    
        public async Task<NextOfKinDto> CreateNextOfKin(NextOfKinDto nextofkinDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                NextOfKin nextofkin = _mapper.Map<NextOfKinDto, NextOfKin>(nextofkinDto);
                var addedNextOfKin = db.NextOfKin.Add(nextofkin);
                await db.SaveChangesAsync();
                return _mapper.Map<NextOfKin, NextOfKinDto>(addedNextOfKin.Entity);
            }
        }
    
        public async Task<IEnumerable<NextOfKinDto>> GetAllNextOfKin()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<NextOfKinDto> nextofkinDtos =
                        _mapper.Map<IEnumerable<NextOfKin>, IEnumerable<NextOfKinDto>>(db.NextOfKin);
                    return nextofkinDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<NextOfKinDto> GetNextOfKin(string userid)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    NextOfKinDto nextofkinDto =
                        _mapper.Map<NextOfKin, NextOfKinDto>(await db.NextOfKin.FirstOrDefaultAsync(x => x.UserId == userid));
                    //if (nextofkinDto == null) { return null; }
                    return nextofkinDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteNextOfKin(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var nextofkin = db.NextOfKin.FirstOrDefault(x => x.Id == Id);
                if (nextofkin != null)
                {
                    db.NextOfKin.Remove(nextofkin);
                    db.SaveChanges();
                }
            }
        }

        public async Task<NextOfKinDto> UpdateNextOfKin(NextOfKinDto nextofkinDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                  var nextofkin = await db.NextOfKin.FirstOrDefaultAsync(x => x.UserId == nextofkinDto.UserId);
                    if (nextofkin == null)
                    {
                        return await CreateNextOfKin(nextofkinDto);
                    }
                    else
                    {
                        _mapper.Map(nextofkinDto, nextofkin);
                        db.NextOfKin.Update(nextofkin);
                         await db.SaveChangesAsync();
                         return _mapper.Map<NextOfKinDto>(nextofkin);
                         
                    }
                }
            }
            catch (Exception ex)
            {
                  Console.WriteLine($"Error updating biodata for user {nextofkinDto.UserId}: {ex.Message}");
                return null;
                
            }
        }
    }

}
