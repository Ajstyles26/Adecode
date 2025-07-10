using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class SubjectRepository : ISubjectRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public SubjectRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<SubjectDto> CreateSubject(SubjectDto subjectDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Subject subject = _mapper.Map<SubjectDto, Subject>(subjectDto);
            var addedSubject = db.Subject.Add(subject);
            await db.SaveChangesAsync();
            return _mapper.Map<Subject, SubjectDto>(addedSubject.Entity);
        }
    }

    public async Task<IEnumerable<SubjectDto>> GetAllSubject()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<SubjectDto> subjectDtos =
                    _mapper.Map<IEnumerable<Subject>, IEnumerable<SubjectDto>>(db.Subject);
                return subjectDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<SubjectDto> GetSubject(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                SubjectDto subjectDto =
                    _mapper.Map<Subject, SubjectDto>(await db.Subject.FirstOrDefaultAsync(x => x.Id == Id));
                return subjectDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteSubject(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var currSubject = db.Subject.FirstOrDefault(x => x.Id == Id);
            if (currSubject != null)
            {
                db.Subject.Remove(currSubject);
                db.SaveChanges();
            }
        }
    }

    public async Task<SubjectDto> UpdateSubject(SubjectDto subjectDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Subject subject = db.Subject.FirstOrDefault(x => x.Id == subjectDto.Id);
                if (subject == null)
                {
                    return null;
                }
                else
                {
                    Subject subjectUpdate = _mapper.Map<SubjectDto, Subject>(subjectDto, subject);
                    var currentupdate = db.Subject.Update(subjectUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Subject, SubjectDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
