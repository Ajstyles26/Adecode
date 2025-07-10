using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IApplicationProgramRepository
    {
        Task<ApplicationProgramDto> CreateApplicationProgram(ApplicationProgramDto ApplicationProgramDto);
        Task<IEnumerable<ApplicationProgramDto>> GetAllApplicationProgram();
        Task<ApplicationProgramDto> GetApplicationProgram(long Id);
        Task DeleteApplicationProgram(long Id);
        Task<ApplicationProgramDto> UpdateApplicationProgram(ApplicationProgramDto ApplicationProgramDto);
    }
}
