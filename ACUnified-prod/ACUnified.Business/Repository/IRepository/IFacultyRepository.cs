using ACUnified.Data.DTOs;

public interface IFacultyRepository
{
    Task<FacultyDto> CreateFaculty(FacultyDto facultyDto);
    Task<IEnumerable<FacultyDto>> GetAllFaculty();
    Task<FacultyDto> GetFaculty(long Id);
    Task DeleteFaculty(long Id);
    Task<FacultyDto> UpdateFaculty(FacultyDto facultyDto);
}
