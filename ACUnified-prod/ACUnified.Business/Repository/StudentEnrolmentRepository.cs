using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Business.Repository;

public class StudentEnrolmentRepository : IStudentEnrolmentRepository
{
    private readonly DbContextOptions<ApplicationDbContext> dbOptions;
    private readonly IMapper _mapper;
    public StudentEnrolmentRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
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
                StudentDto  studentDto = _mapper.Map<Student, StudentDto>(await db.Student
                    .FirstOrDefaultAsync(x => x.Id == Id));
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
                StudentDto studentDto = _mapper.Map<Student, StudentDto>(await db.Student.Include(x => x.CurrentUser)
                    .FirstOrDefaultAsync(x => x.CurrentUser.Id == Id));
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
    public async Task<StudentResultDto> GetResult(StudentDto studentResultDto)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<StudentEnrolment> student = db.StudentEnrolment.Include(x => x.Student).Include(x=>x.Degree).Include(x=>x.ProgramSetup).ToList();
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

    public Task<StudentEnrolmentDto> CreateStudentEnrolment(StudentEnrolmentDto studentDto)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StudentEnrolmentDto>> GetAllStudentEnrolment()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {


                IEnumerable<StudentEnrolmentDto> studentEnrolmentDtos =
                   _mapper.Map<IEnumerable<StudentEnrolment>, IEnumerable<StudentEnrolmentDto>>(db.StudentEnrolment.Include(x => x.Degree)
                   .Include(x=>x.ProgramSetup).Include(x=>x.Level).Include(x=>x.Student));
                return  studentEnrolmentDtos;
            }
        }
        catch(Exception e)
        {
            return null;
        }
    }

    public Task<StudentEnrolmentDto> GetStudentEnrolment(long Id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StudentEnrolmentDto>> GetStudentEnrolmentByDegree(long Id)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {


                IEnumerable<StudentEnrolmentDto> studentEnrolmentDtos =
                   _mapper.Map<IEnumerable<StudentEnrolment>, IEnumerable<StudentEnrolmentDto>>(db.StudentEnrolment.Include(x => x.Degree)
                   .Include(x => x.ProgramSetup).Include(x => x.Level).Include(x => x.Student).Where(x=>x.DegreeId==Id));
                return studentEnrolmentDtos;
            }
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public Task<StudentEnrolmentDto> DeleteStudentEnrolment(long Id)
    {
        throw new NotImplementedException();
    }

    public Task<StudentEnrolmentDto> UpdateStudentEnrolment(StudentEnrolmentDto studentDto)
    {
        throw new NotImplementedException();
    }

    public Task<StudentResultDto> GetResult(StudentEnrolmentDto studentResultDto)
    {
        throw new NotImplementedException();
    }

    public Task<StudentEnrolmentDto> GetStudentEnrolledByUserId(string Id)
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            StudentEnrolment student = db.StudentEnrolment.FirstOrDefault(x => x.Student.UserId == Id);
            if (student == null)
            {
                return null;
            }
            else
            {

                return null;
            }
        }
        return null;
    }

    public Task<IEnumerable<StudentEnrolmentDto>> GetAllStudentEnrolmentCourseReg()
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<StudentEnrolmentDto>> GetAllStudentEnrolmentByDPLE(long degreeId,long programSetupId,long levelId,EntryMode entrymode)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {


                IEnumerable<StudentEnrolmentDto> studentEnrolmentDtos =
                   _mapper.Map<IEnumerable<StudentEnrolment>, IEnumerable<StudentEnrolmentDto>>(db.StudentEnrolment.Include(x => x.Student).Where(x=>x.DegreeId==degreeId
                   &&x.LevelId==levelId && x.ProgramSetupId==programSetupId
                   &&x.Student.StudentEntryMode==entrymode)
                   .Include(x => x.Degree)
                   .Include(x => x.ProgramSetup).Include(x => x.Level));
                return studentEnrolmentDtos;
            }
        }
        catch (Exception e)
        {
            return null;
        }
    }
    public Task<StudentEnrolmentDto> GetStudentEnrolledByStudentId(long Id)
    {
        throw new NotImplementedException();
    }

    public Task<StudentEnrolmentDto> UpdateStudent(StudentEnrolmentDto studentDto)
    {
        throw new NotImplementedException();
    }
}