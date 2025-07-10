using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class StateRepository : IStateRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public StateRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<StateDto> CreateState(StateDto stateDto)
    {
        using (var db=new ApplicationDbContext(dbOptions))
        {
             State state = _mapper.Map<StateDto, State>(stateDto);
                    var addedState = db.State.Add(state);
                    await db.SaveChangesAsync();
                    return _mapper.Map<State, StateDto>(addedState.Entity);
        }
       
    }

    public async Task<IEnumerable<StateDto>> GetAllState()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<StateDto> stateDtos = _mapper.Map<IEnumerable<State>, IEnumerable<StateDto>>(db.State);
                return stateDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<StateDto> GetState(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                StateDto stateDto = _mapper.Map<State, StateDto>(await db.State.FirstOrDefaultAsync(x => x.Id == Id));
                return stateDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task   DeleteState(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var currState = db.State.FirstOrDefault(x => x.Id == Id);
            if (currState != null)
            {
                db.State.Remove(currState);
                db.SaveChanges();
            }
        }
    }

    public async Task<StateDto> UpdateState(StateDto stateDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                State state = db.State.FirstOrDefault(x => x.Id == stateDto.Id);
                if (state == null)
                {
                    return null;
                }
                else
                {
                    State stateUpdate = _mapper.Map<StateDto, State>(stateDto, state);
                    var currentupdate = db.State.Update(stateUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<State, StateDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
