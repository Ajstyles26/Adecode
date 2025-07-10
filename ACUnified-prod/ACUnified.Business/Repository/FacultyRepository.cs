using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


public class FacultyRepository : IFacultyRepository
{
    
    private readonly IMapper _mapper;
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    public FacultyRepository( IMapper mapper,DbContextOptions<ApplicationDbContext> options)
    {
        _mapper = mapper;
        
        dbOptions = options;
    }

    public async Task<FacultyDto> CreateFaculty(FacultyDto facultyDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            Faculty faculty = _mapper.Map<FacultyDto, Faculty>(facultyDto);
            var addedFaculty = db.Faculty.Add(faculty);
            await db.SaveChangesAsync();
            return _mapper.Map<Faculty, FacultyDto>(addedFaculty.Entity);
        }
        
    }

    public async Task<IEnumerable<FacultyDto>> GetAllFaculty()
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                IEnumerable<FacultyDto> facultyDtos = _mapper.Map<IEnumerable<Faculty>, IEnumerable<FacultyDto>>(db.Faculty);
                //var facultyNames = facultyDtos.Select(x => x.Name).ToList();
                //db.Faculty.Where(x => facultyNames.Contains(x.Name));
                return  facultyDtos;
            }
            
           
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> FacultyExists(FacultyDto facultyDto)
    {
        if (facultyDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                return await db.Faculty.AnyAsync(f => f.Name == facultyDto.Name);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    public async Task<FacultyDto> GetFaculty(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                FacultyDto facultyDto =
                    _mapper.Map<Faculty, FacultyDto>(await db.Faculty.FirstOrDefaultAsync(x => x.Id == Id));
                return facultyDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteFaculty(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var faculty = db.Faculty.FirstOrDefault(x => x.Id == Id);
            if (faculty != null)
            {
                db.Faculty.Remove(faculty);
                db.SaveChanges();
            }
        }
    }

    public async Task<FacultyDto> UpdateFaculty(FacultyDto facultyDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Faculty faculty = db.Faculty.FirstOrDefault(x => x.Id == facultyDto.Id);
                if (faculty == null)
                {
                    return null;
                }
                else
                {
                    Faculty facultyUpdate = _mapper.Map<FacultyDto, Faculty>(facultyDto, faculty);
                    var currentupdate = db.Faculty.Update(facultyUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Faculty, FacultyDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
