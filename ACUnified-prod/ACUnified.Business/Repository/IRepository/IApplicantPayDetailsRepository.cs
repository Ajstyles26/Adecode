using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;

public interface IApplicantPayDetailsRepository
{
    Task<ApplicantPayDetailsDto> CreatePayDetails(ApplicantPayDetailsDto ApplicantPayDetailsDto);
    Task<IEnumerable<ApplicantPayDetailsDto>> GetAllPayDetails();

    //   Task<IEnumerable<ApplicantPayDetailsDto>> GetPayDetailsByProgramSetupId(long programSetupId);
  
    Task<ApplicantPayDetailsDto> GetPayDetails(long Id);
    Task<ApplicantPayDetailsDto> GetPayDetailsByName(string name);
    Task DeletePayDetails(long Id);

      Task<IEnumerable<ApplicantPayDetailsDto>> GetPayDetailsByProgramSetupId(long programSetupId);

    Task<ApplicantPayDetailsDto> UpdatePayDetails(ApplicantPayDetailsDto PayDetailsDto);
    Task<IEnumerable<ApplicantPayDetailsDto>> GetStudentPayDetails(SemesterType currentSemester, long categoryId, bool isSemesterLate);

}
