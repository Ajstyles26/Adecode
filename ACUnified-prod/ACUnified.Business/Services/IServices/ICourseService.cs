using System.Collections.Generic;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;

namespace ACUnified.Business.Services.IServices
{
    public interface ICourseService
    {
        Task<CourseDto> CreateCourse(CourseDto courseDto);
        Task<CourseDto> UpdateCourse(CourseDto courseDto);
        Task DeleteCourse(long id);
        Task<CourseDto> GetCourse(long id);
        Task<CourseDto> GetCourseByCode(string courseCode);
        Task<IEnumerable<CourseDto>> GetAllCourses();
        Task<IEnumerable<CourseDto>> GetCoursesByDepartment(long departmentId);
        Task<IEnumerable<CourseDto>> GetStudentCourses(long departmentId, long levelId, SemesterType semesterId);
        Task<IEnumerable<CourseDto>> SearchCoursesByName(string searchText);
    }
}