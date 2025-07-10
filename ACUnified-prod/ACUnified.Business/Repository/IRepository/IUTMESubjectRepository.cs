using ACUnified.Data.DTOs;

public interface IUTMESubjectRepository
{
    Task<UTMESubjectDto> CreateUTMESubject(UTMESubjectDto subjectdetailsDto);
    Task<IEnumerable<UTMESubjectDto>> GetAllUTMESubject();
    Task<UTMESubjectDto> GetUTMESubject(long Id);
    Task DeleteUTMESubject(long Id);
    Task<UTMESubjectDto> UpdateUTMESubject(UTMESubjectDto subjectsetupDto);
}
