using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Business.Services
{
    public class CourseRegistrationService : ICourseRegistrationService
    {
        private readonly ICourseRegistrationRepository _courseRegistrationRepository;
        private readonly ICourseService _courseService;
        private readonly IProgramSetupRepository _programSetupRepository;
        private readonly IAcademicSessionRepository _academicSessionRepository;
        private readonly IAcademicSemesterRepository _academicSemesterRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public CourseRegistrationService(
            ICourseRegistrationRepository courseRegistrationRepository,
            ICourseService courseService,
            IProgramSetupRepository programSetupRepository,
            IAcademicSessionRepository academicSessionRepository,
            IAcademicSemesterRepository academicSemesterRepository,
            IMapper mapper,
            ApplicationDbContext dbContext)
        {
            _courseRegistrationRepository = courseRegistrationRepository;
            _courseService = courseService;
            _programSetupRepository = programSetupRepository;
            _academicSessionRepository = academicSessionRepository;
            _academicSemesterRepository = academicSemesterRepository;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<bool> AutoRegisterCoursesForStudents()
        {
            try
            {
var courseCodes = new[] {
            "ACU GST101",
            "GST111",
            "COS101",
            "BIO101",
            "BIO107",
            "CHM101",
            "CHM107",
            "PHY101",
            "PHY107",
            "MTH101"
        };

                var activeSession = (await _academicSessionRepository.GetActiveSession())?.FirstOrDefault();
                var activeSemester = (await _academicSemesterRepository.GetActiveSemester())?.FirstOrDefault();
                if (activeSession == null || activeSemester == null)
                {
                    throw new InvalidOperationException("No active session or semester found.");
                }

                long levelId = 1;

             var students = await _dbContext.ApplicationForm
                    .Include(a => a.ProgramSetup)
                    .ThenInclude(a => a.Department)
                    .Where(a => a.MatriculationNumber != null &&
                                a.MatriculationNumber.StartsWith("24BMS04") &&
                                a.LevelId == levelId)
                    .ToListAsync();

                if (!students.Any())
                {
                    throw new InvalidOperationException("No eligible students found.");
                }

                foreach (var student in students)
                {
                    var programSetup = await _programSetupRepository.GetProgramSetup(student.ProgramSetupId.Value);
                    if (programSetup == null)
                    {
                        continue;
                    }

                    var existingRegistrations = await _courseRegistrationRepository
                        .GetCourseRegistrationByApplicationFormId(student.Id);

                    var coursesToRegister = new List<CourseDto>();
                    foreach (var courseCode in courseCodes)
                    {
                        var course = await _courseService.GetCourseByCode(courseCode);
                        if (course == null)
                        {
                            continue;
                        }

                        if (existingRegistrations.Any(r => r.CourseId == course.Id))
                        {
                            continue;
                        }

                        coursesToRegister.Add(course);
                    }

                    int totalCourses = coursesToRegister.Count;
                    int totalUnits = coursesToRegister.Sum(c => c.CourseUnit);

                    foreach (var course in coursesToRegister)
                    {
                        var courseRegistration = new CourseRegistrationDto
                        {
                            ApplicationFormId = student.Id,
                            CourseId = course.Id,
                            SessionId = activeSession.Id,
                            SemesterId = activeSemester.Id,
                            ProgramSetupId = student.ProgramSetupId.Value,
                            TotalCourses = totalCourses,
                            TotalUnits = totalUnits,
                            InstitutionProviderId = programSetup.InstitutionProviderId
                        };

                        await _courseRegistrationRepository.CreateCourseRegistration(courseRegistration);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to auto-register courses: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseRegistrationDto>> GetCourseRegistrationByApplicationFormId(long applicationFormId)
        {
            try
            {
                var registrations = await _courseRegistrationRepository.GetCourseRegistrationByApplicationFormId(applicationFormId);
                return registrations ?? Enumerable.Empty<CourseRegistrationDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get course registrations for ApplicationFormId {applicationFormId}: {ex.Message}", ex);
            }
        }

        public async Task<CourseRegistrationDto> CreateCourseRegistration(CourseRegistrationDto courseRegistration)
        {
            try
            {
                if (courseRegistration == null || courseRegistration.CourseId <= 0 || courseRegistration.ApplicationFormId <= 0)
                {
                    throw new ArgumentException("Invalid course registration data: CourseId and ApplicationFormId are required.");
                }

                var result = await _courseRegistrationRepository.CreateCourseRegistration(courseRegistration);
                if (result == null)
                {
                    throw new InvalidOperationException("Failed to create course registration.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create course registration: {ex.Message}", ex);
            }
        }
        

        public async Task DeleteCourseRegistrationByApplicationFormAndCourse(long applicationFormId, long courseId)
        {
            try
            {
                await _courseRegistrationRepository.DeleteCourseRegistrationByApplicationFormAndCourse(applicationFormId, courseId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete course registration for ApplicationFormId {applicationFormId} and CourseId {courseId}: {ex.Message}", ex);
            }
        }
    }
}