using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Business.Repository;

public class SubjectSetupRepository : ISubjectSetupRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public SubjectSetupRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<SubjectSetupDto> CreateSubjectSetup(SubjectSetupDto subjectsetupDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            SubjectSetup subjectsetup = _mapper.Map<SubjectSetupDto, SubjectSetup>(subjectsetupDto);
            var addedSubjectSetup = db.SubjectSetup.Add(subjectsetup);
            await db.SaveChangesAsync();
            return _mapper.Map<SubjectSetup, SubjectSetupDto>(addedSubjectSetup.Entity);
        }
    }

    public async Task<IEnumerable<SubjectSetupDto>> GetAllSubjectSetup()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<SubjectSetupDto> subjectsetupDtos =
                    _mapper.Map<IEnumerable<SubjectSetup>, IEnumerable<SubjectSetupDto>>(db.SubjectSetup .OrderBy(a=>a.Name));
                return subjectsetupDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<SubjectSetupDto> GetSubjectSetup(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                SubjectSetupDto subjectsetupDto =
                    _mapper.Map<SubjectSetup, SubjectSetupDto>(
                        await db.SubjectSetup.FirstOrDefaultAsync(x => x.Id == Id));
                return subjectsetupDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteSubjectSetup(long Id)
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

    public async Task<SubjectSetupDto> UpdateSubjectSetup(SubjectSetupDto subjectsetupDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                SubjectSetup subjectsetup = db.SubjectSetup.FirstOrDefault(x => x.Id == subjectsetupDto.Id);
                if (subjectsetup == null)
                {
                    return null;
                }
                else
                {
                    SubjectSetup subjectdetailsUpdate =
                        _mapper.Map<SubjectSetupDto, SubjectSetup>(subjectsetupDto, subjectsetup);
                    var currentupdate = db.SubjectSetup.Update(subjectdetailsUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<SubjectSetup, SubjectSetupDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

   
}