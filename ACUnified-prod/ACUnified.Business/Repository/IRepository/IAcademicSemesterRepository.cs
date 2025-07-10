using ACUnified.Data.DTOs;

public interface IAcademicSemesterRepository
{
    Task<SemesterDto> CreateSemester(SemesterDto SemesterDto);
    Task<IEnumerable<SemesterDto>> GetAllSemester();
    Task<SemesterDto> GetSemester(long Id);
    Task DeleteSemester(long Id);
    Task<SemesterDto> UpdateSemester(SemesterDto SemesterDto);
    Task<IEnumerable<SemesterDto>>  GetActiveSemester();
    Task<IEnumerable<SemesterDto>> GetActiveApplicantSemester();
}
