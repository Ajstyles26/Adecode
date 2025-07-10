using ACUnified.Data.DTOs;

public interface IAcademicSessionRepository
{
    Task<SessionDto> CreateSession(SessionDto sessionDto);
    Task<IEnumerable<SessionDto>> GetAllSession();
    Task<SessionDto> GetSession(long Id);
    Task DeleteSession(long Id);
    Task<SessionDto> UpdateSession(SessionDto sessionDto);
    Task<IEnumerable<SessionDto>>  GetActiveSession();
    Task<IEnumerable<SessionDto>> GetActiveApplicantSession();
}
