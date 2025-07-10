using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class LGARepository : ILGARepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public LGARepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<LGADto> CreateLGA(LGADto lgaDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            LGA lga = _mapper.Map<LGADto, LGA>(lgaDto);
            var addedLGA = db.LGA.Add(lga);
            await db.SaveChangesAsync();
            return _mapper.Map<LGA, LGADto>(addedLGA.Entity);
        }
    }

    public async Task<IEnumerable<LGADto>> GetAllLGA()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<LGADto> lgaDtos = _mapper.Map<IEnumerable<LGA>, IEnumerable<LGADto>>(db.LGA);
                return lgaDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<LGADto> GetLGA(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                LGADto lgaDto = _mapper.Map<LGA, LGADto>(await db.LGA.FirstOrDefaultAsync(x => x.Id == Id));
                return lgaDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteLGA(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var lga = db.LGA.FirstOrDefault(x => x.Id == Id);
            if (lga != null)
            {
                db.LGA.Remove(lga);
                db.SaveChanges();
            }
        }
    }

    public async Task<LGADto> UpdateLGA(LGADto lgaDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                LGA lga = db.LGA.FirstOrDefault(x => x.Id == lgaDto.Id);
                if (lga == null)
                {
                    return null;
                }
                else
                {
                    LGA lgaUpdate = _mapper.Map<LGADto, LGA>(lgaDto, lga);
                    var currentupdate = db.LGA.Update(lgaUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<LGA, LGADto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}