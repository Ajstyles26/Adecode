using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;

public class SemesterRepository : ISemesterRepository
{
    
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public SemesterRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<SemesterDto> CreateSemester(SemesterDto SemesterDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
            Semester Semester = _mapper.Map<SemesterDto, Semester>(SemesterDto);
            var addedSemester = db.Semester.Add(Semester);
            await db.SaveChangesAsync();
            return _mapper.Map<Semester, SemesterDto>(addedSemester.Entity);
        }
    }

    public async Task<IEnumerable<SemesterDto>> GetAllSemester()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<SemesterDto> SemesterDtos =
                    _mapper.Map<IEnumerable<Semester>, IEnumerable<SemesterDto>>(db.Semester);
                return SemesterDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<SemesterDto> GetSemester(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                SemesterDto SemesterDto =
                    _mapper.Map<Semester, SemesterDto>(await db.Semester.FirstOrDefaultAsync(x => x.Id == Id));
                return SemesterDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    public async Task<IEnumerable<SemesterDto>> GetActiveSemester()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
               
                IEnumerable<SemesterDto> semesterDto =  _mapper.Map<IEnumerable<Semester>, IEnumerable<SemesterDto>>(  db.Semester.Where(x => x.isActive));
                return semesterDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteSemester(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var acada_Semester = db.Semester.FirstOrDefault(x => x.Id == Id);
            if (acada_Semester != null)
            {
                db.Semester.Remove(acada_Semester);
                db.SaveChanges();
            }
        }
    }

    public async Task<SemesterDto> UpdateSemester(SemesterDto SemesterDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Semester Semester = db.Semester.FirstOrDefault(x => x.Id == SemesterDto.Id);
                if (Semester == null)
                {
                    return null;
                }
                else
                {
                    Semester SemesterUpdate = _mapper.Map<SemesterDto, Semester>(SemesterDto, Semester);
                    var currentupdate = db.Semester.Update(SemesterUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Semester, SemesterDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
