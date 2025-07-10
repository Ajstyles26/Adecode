using ACUnified.Data.Enum;
using ACUnified.Data.DTOs;

public interface ICourseRepository
{
    Task<CourseDto> CreateCourse(CourseDto courseDto);
    Task<CourseDto> UpdateCourse(CourseDto courseDto);
    Task<IEnumerable<CourseDto>> GetCoursesByDepartment(long departmentId);
    Task<IEnumerable<CourseDto>> GetAllCourse();
    Task<CourseDto> GetCourse(long id);
    Task<IEnumerable<CourseDto>> GetStudentCourse(long departmentId, long levelId, SemesterType semesterId);
    Task<IEnumerable<CourseDto>> GetCourseByName(string searchText);
    Task<IEnumerable<CourseDto>> GetStudentCourse2(long departmentId, long levelId, SemesterType semesterId);
    Task DeleteCourse(long id);
    Task<CourseDto> GetCourseByCode(string courseCode); // New method
}