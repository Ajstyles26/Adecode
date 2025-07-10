using System.Collections.Generic;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;

namespace ACUnified.Business.Services.IServices
{
    public interface ICourseRegistrationService
    {
        Task<bool> AutoRegisterCoursesForStudents();
        Task<IEnumerable<CourseRegistrationDto>> GetCourseRegistrationByApplicationFormId(long applicationFormId);
        Task<CourseRegistrationDto> CreateCourseRegistration(CourseRegistrationDto courseRegistration);
        Task DeleteCourseRegistrationByApplicationFormAndCourse(long applicationFormId, long courseId);
    }
}