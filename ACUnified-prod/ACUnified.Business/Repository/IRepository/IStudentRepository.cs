using ACUnified.Data.DTOs;

public interface IStudentRepository
{
    Task<StudentDto> CreateStudent(StudentDto studentDto);
    Task<IEnumerable<StudentDto>> GetAllStudent();
        Task<StudentEnrolmentDto> GetStudentEnrollment(long studentId);
    Task<StudentDto> GetStudent(long Id);
    Task<StudentDto> DeleteStudent(long Id);
    Task<StudentDto> UpdateStudent(StudentDto studentDto);
    Task<StudentResultDto> GetResult(StudentDto studentResultDto);
    Task<StudentDto> GetStudentByUserId(string Id);
    Task<IEnumerable<StudentDto>> GetAllStudentCourseReg();
    Task<IEnumerable<StudentDto>> GetAllUnMigratedStudent();
}
