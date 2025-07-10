using AutoMapper;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.Models;
using ACUnified.Data.DTOs;


public class ProgramSetupRepository : IProgramSetupRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public ProgramSetupRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<ProgramSetupDto> CreateProgramSetup(ProgramSetupDto programSetupDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            ProgramSetup programSetup = _mapper.Map<ProgramSetupDto, ProgramSetup>(programSetupDto);
            var addedProgramSetup = db.ProgramSetup.Add(programSetup);
            await db.SaveChangesAsync();
            return _mapper.Map<ProgramSetup, ProgramSetupDto>(addedProgramSetup.Entity);
        }
    }

    public async Task<IEnumerable<ProgramSetupDto>> GetAllProgramSetup()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ProgramSetupDto> programSetupDtos =
                    _mapper.Map<IEnumerable<ProgramSetup>, IEnumerable<ProgramSetupDto>>(db.ProgramSetup
                        .Include(x => x.Faculty).Include(x => x.Department)).OrderBy(a=>a.Name);
                return programSetupDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> ProgramSetupExists(ProgramSetupDto programSetupDto)
    {
        if (programSetupDto == null) return false;
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {

                return await db.ProgramSetup.AnyAsync(f => f.Name == programSetupDto.Name);
            }
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

 public async Task<ProgramSetupDto> GetProgramSetup(long Id)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var programSetup = await db.ProgramSetup
                .Include(p => p.Department)
                .Include(p => p.Faculty)
                .FirstOrDefaultAsync(x => x.Id == Id);

            return _mapper.Map<ProgramSetup, ProgramSetupDto>(programSetup);
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        return null;
    }
}

    public async Task DeleteProgramSetup(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var programSetup = db.ProgramSetup.FirstOrDefault(x => x.Id == Id);
            if (programSetup != null)
            {
                db.ProgramSetup.Remove(programSetup);
                db.SaveChanges();
            }
        }
    }

    public async Task<ProgramSetupDto> UpdateProgramSetup(ProgramSetupDto programSetupDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ProgramSetup programSetup = db.ProgramSetup.FirstOrDefault(x => x.Id == programSetupDto.Id);
                if (programSetup == null)
                {
                    return null;
                }
                else
                {
                    ProgramSetup programSetupUpdate =
                        _mapper.Map<ProgramSetupDto, ProgramSetup>(programSetupDto, programSetup);
                    var currentupdate = db.ProgramSetup.Update(programSetupUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<ProgramSetup, ProgramSetupDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
