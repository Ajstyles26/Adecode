using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


public class DegreeRepository : IDegreeRepository
{
    
    private readonly IMapper _mapper;
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    public DegreeRepository( IMapper mapper,DbContextOptions<ApplicationDbContext> options)
    {
        _mapper = mapper;
        
        dbOptions = options;
    }

    public async Task<DegreeDto> CreateDegree(DegreeDto DegreeDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            Degree Degree = _mapper.Map<DegreeDto, Degree>(DegreeDto);
            var addedDegree = db.Degree.Add(Degree);
            await db.SaveChangesAsync();
            return _mapper.Map<Degree, DegreeDto>(addedDegree.Entity);
        }
        
    }

    public async Task<IEnumerable<DegreeDto>> GetAllDegree()
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                IEnumerable<DegreeDto> DegreeDtos = _mapper.Map<IEnumerable<Degree>, IEnumerable<DegreeDto>>(db.Degree);
                //var DegreeNames = DegreeDtos.Select(x => x.Name).ToList();
                //db.Degree.Where(x => DegreeNames.Contains(x.Name));
                return  DegreeDtos;
            }
            
           
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> DegreeExists(DegreeDto DegreeDto)
    {
        if (DegreeDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                return await db.Degree.AnyAsync(f => f.Name == DegreeDto.Name);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<DegreeDto> GetDegree(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                DegreeDto DegreeDto =
                    _mapper.Map<Degree, DegreeDto>(await db.Degree.FirstOrDefaultAsync(x => x.Id == Id));
                return DegreeDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteDegree(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var Degree = db.Degree.FirstOrDefault(x => x.Id == Id);
            if (Degree != null)
            {
                db.Degree.Remove(Degree);
                db.SaveChanges();
            }
        }
    }

    public async Task<DegreeDto> UpdateDegree(DegreeDto DegreeDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Degree Degree = db.Degree.FirstOrDefault(x => x.Id == DegreeDto.Id);
                if (Degree == null)
                {
                    return null;
                }
                else
                {
                    Degree DegreeUpdate = _mapper.Map<DegreeDto, Degree>(DegreeDto, Degree);
                    var currentupdate = db.Degree.Update(DegreeUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Degree, DegreeDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
