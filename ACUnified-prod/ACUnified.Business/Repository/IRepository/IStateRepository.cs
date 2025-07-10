using ACUnified.Data.DTOs;

public interface IStateRepository
{
    Task<StateDto> CreateState(StateDto stateDto);
    Task<IEnumerable<StateDto>> GetAllState();
    Task<StateDto> GetState(long Id);
    Task DeleteState(long Id);
    Task<StateDto> UpdateState(StateDto stateDto);
}
