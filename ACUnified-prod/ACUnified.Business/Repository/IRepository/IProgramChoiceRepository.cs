using ACUnified.Data.DTOs;

public interface IProgramChoiceRepository
{
    Task<ProgramChoiceDto> CreateProgramChoice(ProgramChoiceDto programchoiceDto);
    Task<IEnumerable<ProgramChoiceDto>> GetAllProgramChoice();
    Task<ProgramChoiceDto> GetProgramChoice(string userId);
    Task DeleteProgramChoice(long Id);
    Task<ProgramChoiceDto> UpdateProgramChoice(ProgramChoiceDto programchoiceDto);
}
