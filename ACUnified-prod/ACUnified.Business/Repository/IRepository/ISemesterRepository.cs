using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository;

public interface ISemesterRepository
{
    
        Task<SemesterDto> CreateSemester(SemesterDto SemesterDto);
        Task<IEnumerable<SemesterDto>> GetAllSemester();
        Task<SemesterDto> GetSemester(long Id);
        Task DeleteSemester(long Id);
        Task<SemesterDto> UpdateSemester(SemesterDto SemesterDto);
        Task<IEnumerable<SemesterDto>>  GetActiveSemester();
    
}