using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

using AutoMapper;

using Microsoft.EntityFrameworkCore;
public class EventRepository : IEventRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public EventRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<EventDto> CreateEvent(EventDto EventDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Event Event = _mapper.Map<EventDto, Event>(EventDto);
            var addedEvent = db.Event.Add(Event);
            await db.SaveChangesAsync();
            return _mapper.Map<Event, EventDto>(addedEvent.Entity);
        }
    }

    public async Task<IEnumerable<EventDto>> GetAllEvent()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<EventDto> EventDtos = _mapper.Map<IEnumerable<Event>, IEnumerable<EventDto>>(db.Event);
                return EventDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<EventDto> GetEvent(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                EventDto EventDto = _mapper.Map<Event, EventDto>(await db.Event.FirstOrDefaultAsync(x => x.Id == Id));
                return EventDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteEvent(long Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var currEvent = db.Event.FirstOrDefault(x => x.Id == Id);
            if (currEvent != null)
            {
                db.Event.Remove(currEvent);
                db.SaveChanges();
            }
        }
    }

    public async Task<EventDto> UpdateEvent(EventDto EventDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Event Event = db.Event.FirstOrDefault(x => x.Id == EventDto.Id);
                if (Event == null)
                {
                    return null;
                }
                else
                {
                    Event EventUpdate = _mapper.Map<EventDto, Event>(EventDto, Event);
                    var currentupdate = db.Event.Update(EventUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Event, EventDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
