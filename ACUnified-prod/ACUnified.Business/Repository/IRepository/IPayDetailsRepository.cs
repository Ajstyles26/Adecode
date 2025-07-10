using ACUnified.Data.Enum;
using ACUnified.Data.DTOs;

public interface IPayDetailsRepository
{
    Task<PayDetailsDto> CreatePayDetails(PayDetailsDto PayDetailsDto);
    Task CreatePayDetails(IEnumerable<PayDetailsDto> PayDetailsDto);
    Task<IEnumerable<PayDetailsDto>> GetAllPayDetails();
    Task<PayDetailsDto> GetPayDetails(long Id);
    Task<PayDetailsDto> GetPayDetailsByName(string Name);
    Task DeletePayDetails(long Id);
    Task<PayDetailsDto> UpdatePayDetails(PayDetailsDto PayDetailsDto);
    Task<IEnumerable<PayDetailsDto>> GetStudentPayDetails(long? curentSession, long currentSemester, long studentId, bool isSemesterLate);

  

}
