using ACUnified.Data.DTOs;

public interface IProgramSetupRepository
{
    Task<ProgramSetupDto> CreateProgramSetup(ProgramSetupDto programSetupDto);
    Task<IEnumerable<ProgramSetupDto>> GetAllProgramSetup();
    Task<ProgramSetupDto> GetProgramSetup(long Id);
    Task DeleteProgramSetup(long Id);
    Task<ProgramSetupDto> UpdateProgramSetup(ProgramSetupDto programSetupDto);
}
