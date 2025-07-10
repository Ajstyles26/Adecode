using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class LevelRepository : ILevelRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public LevelRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<LevelDto> CreateLevel(LevelDto levelDto)
    {
         using (var db =new ApplicationDbContext(dbOptions))
        {
                    Level level = _mapper.Map<LevelDto, Level>(levelDto);
                    var addedLevel = db.Level.Add(level);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Level, LevelDto>(addedLevel.Entity);
        }
      
    }

    public async Task<IEnumerable<LevelDto>> GetAllLevel()
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                IEnumerable<LevelDto> levelDtos = _mapper.Map<IEnumerable<Level>, IEnumerable<LevelDto>>(db.Level);
                            return levelDtos;
            }
            
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<LevelDto> GetLevel(long Id)
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                 LevelDto levelDto = _mapper.Map<Level, LevelDto>(await db.Level.FirstOrDefaultAsync(x => x.Id == Id));
                 return levelDto;
            }
           
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteLevel(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var level = db.Level.FirstOrDefault(x => x.Id == Id);
            if (level != null)
            {
                db.Level.Remove(level);
                db.SaveChanges();
            }
        }
    }

    public async Task<LevelDto> UpdateLevel(LevelDto levelDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Level level = db.Level.FirstOrDefault(x => x.Id == levelDto.Id);
                if (level == null)
                {
                    return null;
                }
                else
                {
                    Level levelUpdate = _mapper.Map<LevelDto, Level>(levelDto, level);
                    var currentupdate = db.Level.Update(levelUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Level, LevelDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
