using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Business.Repository;

public class StudentRepository : IStudentRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public StudentRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
    {
        _mapper = mapper;
        dbOptions = options;
    }

    public async Task<StudentDto> CreateStudent(StudentDto studentDto)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            Student student = _mapper.Map<StudentDto, Student>(studentDto);
            var addedStudent = db.Student.Add(student);
            await db.SaveChangesAsync();
            return _mapper.Map<Student, StudentDto>(addedStudent.Entity);
        }
    }

    public async Task<IEnumerable<StudentDto>> GetAllStudent()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<StudentDto> studentDtos =
                    _mapper.Map<IEnumerable<Student>, IEnumerable<StudentDto>>(db.Student.Include(x=>x.Degree));
                return studentDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }



    public async Task<IEnumerable<StudentDto>> GetAllUnMigratedStudent()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<StudentDto> studentDtos =
                    _mapper.Map<IEnumerable<Student>, IEnumerable<StudentDto>>
                    (db.Student
                    .Where(x=>x.MigrationStatus==MigrationStatus.Unmigrated));
                return studentDtos;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<StudentDto> GetStudent(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var students = await db.Student.Include(x=>x.CurrentUser)
                    .Where(x => x.Id == Id).FirstOrDefaultAsync();
                StudentDto  studentDto = _mapper.Map<Student, StudentDto>(students);
                return studentDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<IEnumerable<StudentDto>> GetAllStudentCourseReg()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<Student> students = await db.Student                    
                    .ToListAsync();
                IEnumerable<StudentDto> studentDto = _mapper.Map<IEnumerable<Student>, IEnumerable<StudentDto>>(students);
                return studentDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<StudentDto> GetStudentByUserId(string Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var studentInfo =  await db.Student.Include(x => x.CurrentUser)
                    .Where(x => x.UserId == Id).FirstOrDefaultAsync();
                StudentDto studentDto = _mapper.Map<Student, StudentDto>(studentInfo);
                return studentDto;
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public Task<StudentDto> DeleteStudent(long Id)
    {
        throw new NotImplementedException();
    }

    public async Task<StudentDto> UpdateStudent(StudentDto studentDto)
    {
        try
        {
            using (var db=new ApplicationDbContext(dbOptions))
            {
                Student student = db.Student.FirstOrDefault(x => x.Id == studentDto.Id);
                if (student == null)
                {
                    return null;
                }
                else
                {
                    Student studentUpdate = _mapper.Map<StudentDto, Student>(studentDto, student);
                    var currentupdate = db.Student.Update(studentUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Student, StudentDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<StudentDto> UpdateImage(StudentDto studentDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                Student student = db.Student.FirstOrDefault(x => x.Id == studentDto.Id);
                if (student == null)
                {
                    return null;
                }
                else
                {
                    Student studentUpdate = _mapper.Map<StudentDto, Student>(studentDto, student);
                    var currentupdate = db.Student.Update(studentUpdate);
                    await db.SaveChangesAsync();
                    return _mapper.Map<Student, StudentDto>(currentupdate.Entity);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
 public async Task<StudentEnrolmentDto> GetStudentEnrollment(long studentId)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var enrollment = await db.StudentEnrolment
                    .Include(x => x.ProgramSetup)
                    .Include(x => x.Degree)
                    .Include(x => x.Level)
                    .FirstOrDefaultAsync(x => x.StudentId == studentId);
                    
                return _mapper.Map<StudentEnrolment, StudentEnrolmentDto>(enrollment);
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<StudentResultDto> GetResult(StudentDto studentResultDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                StudentResult student = db.StudentResult.FirstOrDefault(x => x.MatricNo == studentResultDto.Matric);
                if (student == null)
                {
                    return null;
                }
                else
                {

                    var currentStudentResult = db.StudentResult.Find(studentResultDto.Matric);

                    return _mapper.Map<StudentResult, StudentResultDto>(currentStudentResult);
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}