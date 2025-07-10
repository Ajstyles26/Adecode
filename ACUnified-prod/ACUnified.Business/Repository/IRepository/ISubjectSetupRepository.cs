using ACUnified.Data.DTOs;

public interface ISubjectSetupRepository
{
    Task<SubjectSetupDto> CreateSubjectSetup(SubjectSetupDto subjectdetailsDto);
    Task<IEnumerable<SubjectSetupDto>> GetAllSubjectSetup();
    Task<SubjectSetupDto> GetSubjectSetup(long Id);
    Task DeleteSubjectSetup(long Id);
    Task<SubjectSetupDto> UpdateSubjectSetup(SubjectSetupDto subjectsetupDto);
}
