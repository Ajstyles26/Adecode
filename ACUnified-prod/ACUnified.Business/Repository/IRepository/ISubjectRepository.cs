using ACUnified.Data.DTOs;

public interface ISubjectRepository
{
    Task<SubjectDto> CreateSubject(SubjectDto subjectDto);
    Task<IEnumerable<SubjectDto>> GetAllSubject();
    Task<SubjectDto> GetSubject(long Id);
    Task DeleteSubject(long Id);
    Task<SubjectDto> UpdateSubject(SubjectDto subjectDto);
}
