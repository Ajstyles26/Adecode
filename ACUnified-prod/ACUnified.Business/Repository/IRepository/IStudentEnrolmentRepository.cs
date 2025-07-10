using ACUnified.Business.Repository;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;

public interface IStudentEnrolmentRepository
{
    Task<StudentEnrolmentDto> CreateStudentEnrolment(StudentEnrolmentDto studentDto);
    Task<IEnumerable<StudentEnrolmentDto>> GetAllStudentEnrolment();

    Task<StudentDto> GetStudentByUserId(string Id);
 
    Task<StudentEnrolmentDto> GetStudentEnrolment(long Id);
    Task<IEnumerable<StudentEnrolmentDto>> GetStudentEnrolmentByDegree(long Id);
    Task<StudentEnrolmentDto> DeleteStudentEnrolment(long Id);
    Task<StudentEnrolmentDto> UpdateStudentEnrolment(StudentEnrolmentDto studentDto);
    Task<StudentResultDto> GetResult(StudentEnrolmentDto studentResultDto);
    Task<StudentEnrolmentDto> GetStudentEnrolledByUserId(string Id);
    Task<StudentEnrolmentDto> GetStudentEnrolledByStudentId(long Id);

    Task<StudentEnrolmentDto> UpdateStudent(StudentEnrolmentDto studentDto);

    Task<IEnumerable<StudentEnrolmentDto>> GetAllStudentEnrolmentCourseReg();
    //Degree,ProgramSetup,Level,EntryType
    Task<IEnumerable<StudentEnrolmentDto>> GetAllStudentEnrolmentByDPLE(long degreeId,long programsetupid,long levelid,EntryMode entrymode);

}
