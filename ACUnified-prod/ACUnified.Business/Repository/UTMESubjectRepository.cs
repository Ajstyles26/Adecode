using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Business.Repository;

public class UTMESubjectRepository : IUTMESubjectRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public UTMESubjectRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<UTMESubjectDto> CreateUTMESubject(UTMESubjectDto utmesubjectDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            UTMESubject utmesubject = _mapper.Map<UTMESubjectDto, UTMESubject>(utmesubjectDto);
            var addedUTMESubject = db.UTMESubject.Add(utmesubject);
            await db.SaveChangesAsync();
            return _mapper.Map<UTMESubject, UTMESubjectDto>(addedUTMESubject.Entity);
        }
    }

    public async Task<IEnumerable<UTMESubjectDto>> GetAllUTMESubject()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<UTMESubjectDto> utmesubjectDtos =
                    _mapper.Map<IEnumerable<UTMESubject>, IEnumerable<UTMESubjectDto>>(db.UTMESubject .OrderBy(a=>a.Name));
                return utmesubjectDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<UTMESubjectDto> GetUTMESubject(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                UTMESubjectDto utmesubjectDto =
                    _mapper.Map<UTMESubject, UTMESubjectDto>(
                        await db.UTMESubject.FirstOrDefaultAsync(x => x.Id == Id));
                return utmesubjectDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteUTMESubject(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var program = db.ApplicationProgram.FirstOrDefault(x => x.Id == Id);
            if (program != null)
            {
                db.ApplicationProgram.Remove(program);
                db.SaveChanges();
            }
        }
    }

    public async Task<UTMESubjectDto> UpdateUTMESubject(UTMESubjectDto utmesubjectDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                UTMESubject utmesubject = db.UTMESubject.FirstOrDefault(x => x.Id == utmesubjectDto.Id);
                if (utmesubject == null)
                {
                    return null;
                }
                else
                {
                    UTMESubject subjectdetailsUpdate =
                        _mapper.Map<UTMESubjectDto, UTMESubject>(utmesubjectDto, utmesubject);
                    var currentupdate = db.UTMESubject.Update(subjectdetailsUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<UTMESubject, UTMESubjectDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

   
}