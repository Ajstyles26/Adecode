using System;
using System.Linq;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

public class CourseRepository : ICourseRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;

    public CourseRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<CourseDto> CreateCourse(CourseDto courseDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            try
            {
                // Verify that required relationships exist
                var departmentExists = await db.Department.AnyAsync(d => d.Id == courseDto.DepartmentId);
                var facultyExists = await db.Faculty.AnyAsync(f => f.Id == courseDto.FacultyId);
                var programSetupExists = await db.ProgramSetup.AnyAsync(p => p.Id == courseDto.ProgramSetupId);
                var levelExists = await db.Level.AnyAsync(l => l.Id == courseDto.LevelId);
                var degreeExists = await db.Degree.AnyAsync(d => d.Id == courseDto.DegreeId);

                if (!departmentExists || !facultyExists || !programSetupExists || !levelExists || !degreeExists)
                {
                    throw new InvalidOperationException("One or more required relationships do not exist.");
                }

                // Map and save the course
                var course = _mapper.Map<CourseDto, Course>(courseDto);
                
                // Explicitly set the relationship IDs
                course.DepartmentId = courseDto.DepartmentId;
                course.LevelId = courseDto.LevelId;
                course.DegreeId = courseDto.DegreeId;

                var addedCourse = db.Course.Add(course);
                await db.SaveChangesAsync();

                // Map back to DTO and return
                return _mapper.Map<Course, CourseDto>(addedCourse.Entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create course: {ex.Message}", ex);
            }
        }
    }

    public async Task<CourseDto> UpdateCourse(CourseDto courseDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            try
            {
                var existingCourse = await db.Course.FindAsync(courseDto.Id);
                if (existingCourse == null)
                {
                    return null;
                }

                // Verify that required relationships exist
                var departmentExists = await db.Department.AnyAsync(d => d.Id == courseDto.DepartmentId);
                var facultyExists = await db.Faculty.AnyAsync(f => f.Id == courseDto.FacultyId);
                var programSetupExists = await db.ProgramSetup.AnyAsync(p => p.Id == courseDto.ProgramSetupId);
                var levelExists = await db.Level.AnyAsync(l => l.Id == courseDto.LevelId);
                var degreeExists = await db.Degree.AnyAsync(d => d.Id == courseDto.DegreeId);

                if (!departmentExists || !facultyExists || !programSetupExists || !levelExists || !degreeExists)
                {
                    throw new InvalidOperationException("One or more required relationships do not exist.");
                }

                // Update the existing course
                _mapper.Map(courseDto, existingCourse);

                // Explicitly update the relationship IDs
                existingCourse.DepartmentId = courseDto.DepartmentId;
                existingCourse.LevelId = courseDto.LevelId;
                existingCourse.DegreeId = courseDto.DegreeId;

                var updatedCourse = db.Course.Update(existingCourse);
                await db.SaveChangesAsync();

                return _mapper.Map<Course, CourseDto>(updatedCourse.Entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update course: {ex.Message}", ex);
            }
        }
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesByDepartment(long departmentId)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var courses = await db.Course
                    .Include(x => x.Department)
                    .Where(x => x.DepartmentId == departmentId)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(courses);
            }
        }
        catch (Exception ex)
        {
            return new List<CourseDto>();
        }
    }

    public async Task<IEnumerable<CourseDto>> GetAllCourse()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var courses = await db.Course
                    .Include(x => x.Department)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(courses);
            }
        }
        catch (Exception ex)
        {
            return new List<CourseDto>();
        }
    }

    public async Task<CourseDto> GetCourse(long id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var course = await db.Course
                    .Include(x => x.Department)
                    .FirstOrDefaultAsync(x => x.Id == id);
                return _mapper.Map<Course, CourseDto>(course);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<CourseDto>> GetStudentCourse(long departmentId, long levelId, SemesterType semesterId)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var filteredCourses = await db.Course
                    .Where(x => x.DepartmentId == departmentId && x.Semester == semesterId && x.LevelId == levelId)
                    .Include(x => x.Department)
                    .Include(x => x.Level)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(filteredCourses);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<CourseDto>> GetCourseByName(string courseName)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                courseName = courseName?.Trim().ToLower();
                if (string.IsNullOrEmpty(courseName))
                {
                    return new List<CourseDto>();
                }

                var filteredCourses = await db.Course
                    .Where(x => x.Name.ToLower().Contains(courseName) || x.CourseCode.ToLower().Contains(courseName))
                    .Include(x => x.Department)
                    .Take(10)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(filteredCourses);
            }
        }
        catch (Exception ex)
        {
            return new List<CourseDto>();
        }
    }

    public async Task<IEnumerable<CourseDto>> GetStudentCourse2(long departmentId, long levelId, SemesterType semesterId)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var filteredCourses = await db.Course
                    .Where(x => x.DepartmentId == departmentId && x.Semester == semesterId && x.LevelId == levelId)
                    .Include(x => x.Department)
                    .Include(x => x.Level)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<Course>, IEnumerable<CourseDto>>(filteredCourses);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task DeleteCourse(long id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var course = await db.Course.FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                db.Course.Remove(course);
                await db.SaveChangesAsync();
            }
        }
    }

    public async Task<CourseDto> GetCourseByCode(string courseCode)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var course = await db.Course
                    .Include(x => x.Department)
                    .FirstOrDefaultAsync(x => x.CourseCode == courseCode);
                return _mapper.Map<Course, CourseDto>(course);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}