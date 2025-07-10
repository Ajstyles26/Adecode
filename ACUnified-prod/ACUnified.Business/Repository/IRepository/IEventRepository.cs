using ACUnified.Data.DTOs;

public interface IEventRepository
{
    Task<EventDto> CreateEvent(EventDto eventDto);
    Task<IEnumerable<EventDto>> GetAllEvent();
    Task<EventDto> GetEvent(long Id);
    Task DeleteEvent(long Id);
    Task<EventDto> UpdateEvent(EventDto eventDto);
}
