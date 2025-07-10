
using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Data.DTOs;

public class ApplicationProgramRepository : IApplicationProgramRepository
{
    
    private readonly IMapper _mapper;
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    public ApplicationProgramRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        
            _mapper = mapper;
            dbOptions = options;
    }

    public async Task<ApplicationProgramDto> CreateApplicationProgram(ApplicationProgramDto applicationprogramDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            ApplicationProgram applicationprogram =
                _mapper.Map<ApplicationProgramDto, ApplicationProgram>(applicationprogramDto);
            var addedApplicationProgram = db.ApplicationProgram.Add(applicationprogram);
            await db.SaveChangesAsync();
            return _mapper.Map<ApplicationProgram, ApplicationProgramDto>(addedApplicationProgram.Entity);
        }
    }

    public async Task<IEnumerable<ApplicationProgramDto>> GetAllApplicationProgram()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationProgramDto> applicationprogramDtos =
                    _mapper.Map<IEnumerable<ApplicationProgram>, IEnumerable<ApplicationProgramDto>>(
                        db.ApplicationProgram);
                return applicationprogramDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<ApplicationProgramDto> GetApplicationProgram(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicationProgramDto applicationprogramDto =
                    _mapper.Map<ApplicationProgram, ApplicationProgramDto>(
                        await db.ApplicationProgram.FirstOrDefaultAsync(x => x.Id == Id));
                return applicationprogramDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteProgram(long Id)
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

    public async Task<ApplicationProgramDto> UpdateApplicationProgram(ApplicationProgramDto applicationprogramDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicationProgram applicationprogram =
                    db.ApplicationProgram.FirstOrDefault(x => x.Id == applicationprogramDto.Id);
                if (applicationprogram == null)
                {
                    return null;
                }
                else
                {
                    ApplicationProgram applicationprogramUpdate =
                        _mapper.Map<ApplicationProgramDto, ApplicationProgram>(applicationprogramDto,
                            applicationprogram);
                    var currentupdate = db.ApplicationProgram.Update(applicationprogramUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<ApplicationProgram, ApplicationProgramDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
