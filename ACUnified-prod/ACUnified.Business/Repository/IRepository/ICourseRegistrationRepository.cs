using System.Collections.Generic;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository
{
    public interface ICourseRegistrationRepository
    {
        Task<CourseRegistrationDto> CreateCourseRegistration(CourseRegistrationDto courseRegistrationDto);

        Task<IEnumerable<CourseRegistrationDto>> GetCourseRegistrationByApplicationFormId(long applicationFormId);
        Task DeleteCourseRegistrationByApplicationFormAndCourse(long applicationFormId, long courseId);
        Task<CourseRegistrationDto> CreateApplicationFormCourseRegistration(CourseRegistrationDto courseRegistrationDto);
        Task<List<CourseRegistrationDto>> CreateBulkApplicationFormCourseRegistrations(List<CourseRegistrationDto> courseRegistrationDtos);
        Task<IEnumerable<StudentCourseRegistrationListDto>> GetCourseRegistrationListByApplicationForm(long applicationFormId);
        Task<IEnumerable<CourseDto>> GetCoursesByDepartment(long departmentId);
        Task<List<CourseRegistrationDto>> CreateCourseRegistrations(List<CourseRegistrationDto> courseRegistrationDtos);
        Task<IEnumerable<CourseRegistrationDto>> GetAllCourseRegistration();
        Task<CourseRegistrationDto> GetCourseRegistration(long id);
        Task<IEnumerable<CourseRegistrationDto>> GetCourseRegistrationByMatric(string matric);
        Task<IEnumerable<CourseRegistrationDto>> GetCourseRegistrationByStudentId(long id);
        Task<IEnumerable<CourseRegistrationDto>> GetCourseHistoryByStudentId(long id, long semesterId, long sessionId);
        Task<IEnumerable<CourseRegistrationDto>> GetCourseHistoryByMatric(string matric, long semesterId, long sessionId);
        Task DeleteCourseRegistration(long id);
        Task<CourseRegistrationDto> UpdateCourseRegistration(CourseRegistrationDto courseRegistrationDto);
        Task<IEnumerable<StudentCourseRegistrationListDto>> GetCourseRegistrationListByMatric(string matric);
        Task<IEnumerable<StudentCourseRegistrationListDto>> GetCourseRegistrationListByStudentId(long id);
        Task<IEnumerable<CourseRegistrationDto>> GetAllStudentCourseSummaries();
        // Task<IEnumerable<StudentRegistrationDto>> GetStudentRegistrations();
        Task<int> DeleteDuplicateCourseRegistrations(long applicationFormId);
    }
}