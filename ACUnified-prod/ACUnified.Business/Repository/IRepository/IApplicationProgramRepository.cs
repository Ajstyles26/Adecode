
using ACUnified.Data.DTOs;



public interface IApplicationProgramRepository
{
    Task<ApplicationProgramDto> CreateApplicationProgram(ApplicationProgramDto applicationprogramDto);
    Task<IEnumerable<ApplicationProgramDto>> GetAllApplicationProgram();
    Task<ApplicationProgramDto> GetApplicationProgram(long Id);
    Task DeleteProgram(long Id);
    Task<ApplicationProgramDto> UpdateApplicationProgram(ApplicationProgramDto applicationprogramDto);
}
