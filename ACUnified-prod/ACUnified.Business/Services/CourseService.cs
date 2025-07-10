using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;

namespace ACUnified.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseDto> CreateCourse(CourseDto courseDto)
        {
            try
            {
                if (string.IsNullOrEmpty(courseDto.CourseCode) || courseDto.CourseUnit <= 0)
                {
                    throw new ArgumentException("Invalid course data: CourseCode and CourseUnit are required.");
                }

                return await _courseRepository.CreateCourse(courseDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create course: {ex.Message}", ex);
            }
        }

        public async Task<CourseDto> UpdateCourse(CourseDto courseDto)
        {
            try
            {
                if (courseDto.Id <= 0)
                {
                    throw new ArgumentException("Invalid course ID.");
                }

                return await _courseRepository.UpdateCourse(courseDto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update course: {ex.Message}", ex);
            }
        }

        public async Task DeleteCourse(long id)
        {
            try
            {
                await _courseRepository.DeleteCourse(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete course: {ex.Message}", ex);
            }
        }

        public async Task<CourseDto> GetCourse(long id)
        {
            try
            {
                return await _courseRepository.GetCourse(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get course: {ex.Message}", ex);
            }
        }

        public async Task<CourseDto> GetCourseByCode(string courseCode)
        {
            try
            {
                if (string.IsNullOrEmpty(courseCode))
                {
                    throw new ArgumentException("Course code cannot be empty.");
                }

                return await _courseRepository.GetCourseByCode(courseCode);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get course by code: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseDto>> GetAllCourses()
        {
            try
            {
                return await _courseRepository.GetAllCourse();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get all courses: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByDepartment(long departmentId)
        {
            try
            {
                return await _courseRepository.GetCoursesByDepartment(departmentId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get courses by department: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseDto>> GetStudentCourses(long departmentId, long levelId, SemesterType semesterId)
        {
            try
            {
                return await _courseRepository.GetStudentCourse(departmentId, levelId, semesterId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get student courses: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseDto>> SearchCoursesByName(string searchText)
        {
            try
            {
                return await _courseRepository.GetCourseByName(searchText);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to search courses: {ex.Message}", ex);
            }
        }
    }
}