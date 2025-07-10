using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class ProgramChoiceRepository : IProgramChoiceRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public ProgramChoiceRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<ProgramChoiceDto> CreateProgramChoice(ProgramChoiceDto programchoiceDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            ProgramChoice programchoice = _mapper.Map<ProgramChoiceDto, ProgramChoice>(programchoiceDto);
            var addedProgramChoice = db.ProgramChoice.Add(programchoice);
            await db.SaveChangesAsync();
            return _mapper.Map<ProgramChoice, ProgramChoiceDto>(addedProgramChoice.Entity);
        }
    }

    public async Task<IEnumerable<ProgramChoiceDto>> GetAllProgramChoice()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ProgramChoiceDto> programchoiceDtos =
                    _mapper.Map<IEnumerable<ProgramChoice>, IEnumerable<ProgramChoiceDto>>(db.ProgramChoice);
                return programchoiceDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<ProgramChoiceDto> GetProgramChoice(string UserId)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ProgramChoiceDto programchoiceDto =
                    _mapper.Map<ProgramChoice, ProgramChoiceDto>(
                        await db.ProgramChoice.FirstOrDefaultAsync(x => x.UserId == UserId));
                return programchoiceDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteProgramChoice(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var programchoice = db.ProgramChoice.FirstOrDefault(x => x.Id == Id);
            if (programchoice != null)
            {
                db.ProgramChoice.Remove(programchoice);
                db.SaveChanges();
            }
        }
    }

    public async Task<ProgramChoiceDto> UpdateProgramChoice(ProgramChoiceDto programchoiceDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ProgramChoice programchoice = db.ProgramChoice.FirstOrDefault(x => x.Id == programchoiceDto.Id);
                if (programchoice == null)
                {
                    return null;
                }
                else
                {
                    ProgramChoice programchoiceUpdate =
                        _mapper.Map<ProgramChoiceDto, ProgramChoice>(programchoiceDto, programchoice);
                    var currentupdate = db.ProgramChoice.Update(programchoiceUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<ProgramChoice, ProgramChoiceDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
