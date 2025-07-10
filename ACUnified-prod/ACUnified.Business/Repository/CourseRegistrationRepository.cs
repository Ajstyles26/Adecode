using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ACUnified.Business.Repository.IRepository;

namespace ACUnified.Business.Repository
{
    public class CourseRegistrationRepository : ICourseRegistrationRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbOptions;
        private readonly IMapper _mapper;

        public CourseRegistrationRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _dbOptions = options;
            _mapper = mapper;
        }

        public async Task<CourseRegistrationDto> CreateCourseRegistration(CourseRegistrationDto courseRegistrationDto)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                var courseReg = _mapper.Map<CourseRegistrationDto, CourseRegistration>(courseRegistrationDto);
                var addedCourse = db.CourseRegistration.Add(courseReg);
                await db.SaveChangesAsync();
                return _mapper.Map<CourseRegistration, CourseRegistrationDto>(addedCourse.Entity);
            }
        }

        public async Task<IEnumerable<CourseRegistrationDto>> GetCourseRegistrationByApplicationFormId(long applicationFormId)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var courseRegistrations = await db.CourseRegistration
                        .Where(x => x.ApplicationFormId == applicationFormId)
                        .Include(x => x.ApplicationForm)
                            .ThenInclude(a => a.BioData)
                        .Include(x => x.Course)
                        .Include(x => x.ProgramSetup)
                        .Include(x => x.Semester)
                        .Include(x => x.Session)
                        .ToListAsync();

                    var courseRegistrationDtos = _mapper.Map<IEnumerable<CourseRegistration>, IEnumerable<CourseRegistrationDto>>(courseRegistrations);

                    var totalUnits = courseRegistrations.Sum(cr => cr.Course?.CourseUnit ?? 0);
                    var totalCourses = courseRegistrations.Count;

                    foreach (var dto in courseRegistrationDtos)
                    {
                        dto.TotalUnits = totalUnits;
                        dto.TotalCourses = totalCourses;
                    }

                    return courseRegistrationDtos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get course registrations for ApplicationFormId {applicationFormId}: {ex.Message}", ex);
            }
        }
        
        public async Task<IEnumerable<CourseRegistrationDto>> GetAllStudentCourseSummaries()
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var summaries = await db.CourseRegistration
                        .Include(cr => cr.ApplicationForm)
                            .ThenInclude(a => a.BioData)
                        .Include(cr => cr.Course)
                        .Include(cr => cr.ApplicationForm)
                            .ThenInclude(a => a.ProgramSetup)
                        .GroupBy(cr => new
                        {
                            cr.ApplicationFormId,
                            FormRefNo = cr.ApplicationForm != null ? cr.ApplicationForm.FormRefNo : null,
                            LastName = cr.ApplicationForm != null && cr.ApplicationForm.BioData != null ? cr.ApplicationForm.BioData.LastName : null,
                            FirstName = cr.ApplicationForm != null && cr.ApplicationForm.BioData != null ? cr.ApplicationForm.BioData.FirstName : null,
                            OtherName = cr.ApplicationForm != null && cr.ApplicationForm.BioData != null ? cr.ApplicationForm.BioData.OtherName : null,
                            MatriculationNumber = cr.ApplicationForm != null ? cr.ApplicationForm.MatriculationNumber : null,
                            ProgramName = cr.ApplicationForm != null && cr.ApplicationForm.ProgramSetup != null ? cr.ApplicationForm.ProgramSetup.Name : null
                        })
                        .Select(g => new CourseRegistrationDto
                        {
                            ApplicationFormId = g.Key.ApplicationFormId,
                            ApplicationForm = new ApplicationForm
                            {
                                FormRefNo = g.Key.FormRefNo,
                                MatriculationNumber = g.Key.MatriculationNumber,
                                BioData = new BioData
                                {
                                    LastName = g.Key.LastName,
                                    FirstName = g.Key.FirstName,
                                    OtherName = g.Key.OtherName
                                },
                                ProgramSetup = new ProgramSetup
                                {
                                    Name = g.Key.ProgramName
                                }
                            },
                            TotalCourses = g.Count(),
                            TotalUnits = g.Sum(cr => cr.Course != null ? cr.Course.CourseUnit : 0)
                        })
                        .ToListAsync();

                    return summaries;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get student course summaries: {ex.Message}", ex);
            }
        }

        public async Task<int> DeleteDuplicateCourseRegistrations(long applicationFormId)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                try
                {
                    var registrations = await db.CourseRegistration
                        .Where(cr => cr.ApplicationFormId == applicationFormId)
                        .ToListAsync();

                    var duplicates = registrations
                        .GroupBy(cr => cr.CourseId)
                        .Where(g => g.Count() > 1)
                        .SelectMany(g => g.OrderByDescending(cr => cr.Id).Skip(1))
                        .ToList();

                    if (duplicates.Any())
                    {
                        db.CourseRegistration.RemoveRange(duplicates);
                        await db.SaveChangesAsync();
                        return duplicates.Count;
                    }

                    return 0;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting duplicate course registrations: {ex.Message}", ex);
                }
            }
        }

        public async Task DeleteCourseRegistrationByApplicationFormAndCourse(long applicationFormId, long courseId)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                try
                {
                    var registration = await db.CourseRegistration
                        .FirstOrDefaultAsync(cr => cr.ApplicationFormId == applicationFormId && cr.CourseId == courseId);

                    if (registration != null)
                    {
                        db.CourseRegistration.Remove(registration);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting course registration: {ex.Message}", ex);
                }
            }
        }

        public async Task<CourseRegistrationDto> CreateApplicationFormCourseRegistration(CourseRegistrationDto courseRegistrationDto)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                try
                {
                    var applicationForm = await db.ApplicationForm
                        .Include(a => a.BioData)
                        .FirstOrDefaultAsync(x => x.Id == courseRegistrationDto.ApplicationFormId);

                    if (applicationForm == null)
                    {
                        throw new KeyNotFoundException($"Application form with ID {courseRegistrationDto.ApplicationFormId} not found");
                    }

                    if (applicationForm.BioData == null)
                    {
                        throw new InvalidOperationException($"Application form {courseRegistrationDto.ApplicationFormId} has no associated BioData");
                    }

                    var courseReg = _mapper.Map<CourseRegistrationDto, CourseRegistration>(courseRegistrationDto);
                    var addedCourse = db.CourseRegistration.Add(courseReg);
                    await db.SaveChangesAsync();
                    return _mapper.Map<CourseRegistration, CourseRegistrationDto>(addedCourse.Entity);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error creating application form course registration: {ex.Message}", ex);
                }
            }
        }

        public async Task<List<CourseRegistrationDto>> CreateBulkApplicationFormCourseRegistrations(List<CourseRegistrationDto> courseRegistrationDtos)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                try
                {
                    var registrationsByForm = courseRegistrationDtos
                        .GroupBy(x => x.ApplicationFormId)
                        .ToDictionary(g => g.Key, g => g.ToList());

                    var applicationFormIds = registrationsByForm.Keys.Where(id => id.HasValue).Select(id => id.Value);
                    var existingForms = await db.ApplicationForm
                        .Include(a => a.BioData)
                        .Where(f => applicationFormIds.Contains(f.Id))
                        .ToListAsync();

                    var missingForms = applicationFormIds.Except(existingForms.Select(f => f.Id)).ToList();
                    if (missingForms.Any())
                    {
                        throw new KeyNotFoundException($"Application forms not found: {string.Join(", ", missingForms)}");
                    }

                    var formsWithoutBioData = existingForms.Where(f => f.BioData == null).Select(f => f.Id).ToList();
                    if (formsWithoutBioData.Any())
                    {
                        throw new InvalidOperationException($"Application forms missing BioData: {string.Join(", ", formsWithoutBioData)}");
                    }

                    var courses = _mapper.Map<List<CourseRegistrationDto>, List<CourseRegistration>>(courseRegistrationDtos);
                    db.CourseRegistration.AddRange(courses);
                    await db.SaveChangesAsync();
                    return _mapper.Map<List<CourseRegistration>, List<CourseRegistrationDto>>(courses);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error creating bulk course registrations: {ex.Message}", ex);
                }
            }
        }

        public async Task<IEnumerable<StudentCourseRegistrationListDto>> GetCourseRegistrationListByApplicationForm(long applicationFormId)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var registrationList = await db.CourseRegistration
                        .Include(x => x.ApplicationForm)
                            .ThenInclude(a => a.BioData)
                        .Where(cr => cr.ApplicationFormId == applicationFormId)
                        .GroupBy(cr => new { cr.ApplicationFormId, BioDataId = cr.ApplicationForm.BioDataId })
                        .Select(g => new StudentCourseRegistrationListDto
                        {
                            StudentId = null,
                            Name = $"Application Form {g.Key.ApplicationFormId}"
                        })
                        .ToListAsync();

                    return registrationList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course registration list: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByDepartment(long departmentId)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var filteredCourses = await db.Course
                        .Where(x => x.DepartmentId == departmentId)
                        .Include(x => x.Department)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(filteredCourses);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting courses by department: {ex.Message}", ex);
            }
        }

        public async Task<List<CourseRegistrationDto>> CreateCourseRegistrations(List<CourseRegistrationDto> courseRegistrationDtos)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                try
                {
                    var courses = _mapper.Map<List<CourseRegistrationDto>, List<CourseRegistration>>(courseRegistrationDtos);
                    db.CourseRegistration.AddRange(courses);
                    await db.SaveChangesAsync();
                    return _mapper.Map<List<CourseRegistration>, List<CourseRegistrationDto>>(courses);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error creating course registrations: {ex.Message}", ex);
                }
            }
        }

        public async Task<IEnumerable<CourseRegistrationDto>> GetAllCourseRegistration()
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var registrations = await db.CourseRegistration
                        .Include(x => x.Course)
                        .Include(x => x.ApplicationForm)
                        .Include(x => x.Semester)
                        .Include(x => x.Session)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<CourseRegistration>, IEnumerable<CourseRegistrationDto>>(registrations);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all course registrations: {ex.Message}", ex);
            }
        }

        public async Task<CourseRegistrationDto> GetCourseRegistration(long id)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var registration = await db.CourseRegistration
                        .Include(x => x.Course)
                        .Include(x => x.ApplicationForm)
                        .Include(x => x.Semester)
                        .Include(x => x.Session)
                        .FirstOrDefaultAsync(x => x.Id == id);

                    return _mapper.Map<CourseRegistration, CourseRegistrationDto>(registration);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course registration with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseRegistrationDto>> GetCourseRegistrationByMatric(string matric)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var registrations = await db.CourseRegistration
                        .Include(x => x.Student)
                        .Include(x => x.ProgramSetup)
                        .Include(x => x.Course)
                        .Include(x => x.StudentEnrolment)
                        .Include(x => x.Semester)
                        .Include(x => x.Session)
                        .Where(x => x.Student.Matric == matric)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<CourseRegistration>, IEnumerable<CourseRegistrationDto>>(registrations);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course registrations for matric {matric}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseRegistrationDto>> GetCourseRegistrationByStudentId(long id)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var registrations = await db.CourseRegistration
                        .Include(x => x.Student)
                        .Include(x => x.ProgramSetup)
                        .Include(x => x.Course)
                        .Include(x => x.StudentEnrolment)
                        .Include(x => x.Semester)
                        .Include(x => x.Session)
                        .Where(x => x.StudentId == id)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<CourseRegistration>, IEnumerable<CourseRegistrationDto>>(registrations);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course registrations for student ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseRegistrationDto>> GetCourseHistoryByStudentId(long id, long semesterId, long sessionId)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var registrations = await db.CourseRegistration
                        .Include(x => x.Student)
                        .Include(x => x.ProgramSetup)
                        .Include(x => x.Course)
                        .Include(x => x.StudentEnrolment)
                        .Include(x => x.Semester)
                        .Include(x => x.Session)
                        .Where(x => x.StudentId == id && x.SemesterId == semesterId && x.SessionId == sessionId)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<CourseRegistration>, IEnumerable<CourseRegistrationDto>>(registrations);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course history for student ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CourseRegistrationDto>> GetCourseHistoryByMatric(string matric, long semesterId, long sessionId)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var registrations = await db.CourseRegistration
                        .Include(x => x.Student)
                        .Include(x => x.ProgramSetup)
                        .Include(x => x.Course)
                        .Include(x => x.StudentEnrolment)
                        .Include(x => x.Semester)
                        .Include(x => x.Session)
                        .Where(x => x.Student.Matric == matric && x.SemesterId == semesterId && x.SessionId == sessionId)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<CourseRegistration>, IEnumerable<CourseRegistrationDto>>(registrations);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course history for matric {matric}: {ex.Message}", ex);
            }
        }

        public async Task DeleteCourseRegistration(long id)
        {
            using (var db = new ApplicationDbContext(_dbOptions))
            {
                try
                {
                    var registration = await db.CourseRegistration.FirstOrDefaultAsync(x => x.Id == id);
                    if (registration != null)
                    {
                        db.CourseRegistration.Remove(registration);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error deleting course registration with ID {id}: {ex.Message}", ex);
                }
            }
        }

        public async Task<CourseRegistrationDto> UpdateCourseRegistration(CourseRegistrationDto courseRegistrationDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var registration = await db.CourseRegistration.FirstOrDefaultAsync(x => x.Id == courseRegistrationDto.Id);
                    if (registration == null)
                    {
                        return null;
                    }

                    _mapper.Map(courseRegistrationDto, registration);
                    db.CourseRegistration.Update(registration);
                    await db.SaveChangesAsync();
                    return _mapper.Map<CourseRegistration, CourseRegistrationDto>(registration);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating course registration: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<StudentCourseRegistrationListDto>> GetCourseRegistrationListByMatric(string matric)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var studentDto = await db.CourseRegistration
                        .Include(x => x.Student)
                        .Include(x => x.Session)
                        .Include(x => x.Semester)
                        .Where(sr => sr.Student.Matric == matric)
                        .GroupBy(sr => new { sr.StudentId, sr.Session.Name, sr.SessionId, sr.SemesterId })
                        .Select(g => new StudentCourseRegistrationListDto
                        {
                            StudentId = g.Key.StudentId,
                            SessionId = g.Key.SessionId,
                            SemesterId = g.Key.SemesterId,
                            Name = $"Session {g.Key.Name}, Semester {g.Key.SemesterId}"
                        })
                        .ToListAsync();

                    return studentDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course registration list for matric {matric}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<StudentCourseRegistrationListDto>> GetCourseRegistrationListByStudentId(long id)
        {
            try
            {
                using (var db = new ApplicationDbContext(_dbOptions))
                {
                    var studentDto = await db.CourseRegistration
                        .Include(x => x.Student)
                        .Where(sr => sr.StudentId == id)
                        .GroupBy(sr => new { sr.StudentId, sr.SessionId, sr.SemesterId })
                        .Select(g => new StudentCourseRegistrationListDto
                        {
                            StudentId = g.Key.StudentId,
                            SessionId = g.Key.SessionId,
                            SemesterId = g.Key.SemesterId,
                            Name = $"Session {g.Key.SessionId}, Semester {g.Key.SemesterId}"
                        })
                        .ToListAsync();

                    return studentDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course registration list for student ID {id}: {ex.Message}", ex);
            }
        }
    }
}