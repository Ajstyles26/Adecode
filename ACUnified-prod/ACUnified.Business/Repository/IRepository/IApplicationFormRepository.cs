using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;



namespace ACUnified.Business.Repository.IRepository
{
    public interface IApplicationFormRepository
    {
        Task<ApplicationFormDto> CreateApplicationForm(ApplicationFormDto ApplicationFormDto);
          Task<IEnumerable<ApplicationFormDto>> GetAll();
        //regular
        Task<IEnumerable<ApplicationFormDto>> GetAllApplicationForm();
        Task<IEnumerable<ApplicationFormDto>> GetCompletedApplicationForm();

        Task<IEnumerable<ApplicationFormDto>> GetAuthorizedApplicationForm();
        Task<IEnumerable<ApplicationFormDto>> GetFinalizedApplicationForm();
        Task<IEnumerable<ApplicationFormRankingsDto>> GetApplicationFormsByAppliedCourses();
        Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudents();
        Task<ApplicationFormDto> GetCompletedApplicationForm(string userId);
        Task<IEnumerable<ApplicationFormRankingsDto>> GetAdmittedStudentsReg();
       Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudentsDetailsReg();
        //regular

        //bth
        Task<IEnumerable<ApplicationFormDto>> GetAllApplicationFormBTH();
        Task<IEnumerable<ApplicationFormDto>> GetCompletedApplicationFormBTH();
        Task<IEnumerable<ApplicationFormDto>> GetAuthorizedApplicationFormBTH();
        Task<IEnumerable<ApplicationFormDto>> GetFinalizedApplicationFormBTH();
        Task<IEnumerable<ApplicationFormRankingsDto>> GetApplicationFormsByAppliedCoursesBTH();
        Task<IEnumerable<ApplicationFormRankingsDto>> GetAdmittedStudentsBTH();
         Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudentsDetailsBTH();
        //bth
         Task<string> GetLastUsedNumber();
        //JUPEB
         Task<IEnumerable<ApplicationFormDto>> GetAllApplicationFormJUPEB();
        Task<IEnumerable<ApplicationFormDto>> GetCompletedApplicationFormJUPEB();
        Task<IEnumerable<ApplicationFormDto>> GetAuthorizedApplicationFormJUPEB();
        Task<IEnumerable<ApplicationFormDto>> GetFinalizedApplicationFormJUPEB();
        Task<IEnumerable<ApplicationFormRankingsDto>> GetApplicationFormsByAppliedCoursesJUPEB();
        Task<IEnumerable<ApplicationFormRankingsDto>> GetAdmittedStudentsJUPEB();
         Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudentsDetailsJUPEB();
       //JUPEB

        //PG
        Task<IEnumerable<ApplicationFormDto>> GetAllApplicationFormPG();
        Task<IEnumerable<ApplicationFormDto>> GetCompletedApplicationFormPG();

        Task<IEnumerable<ApplicationFormDto>> GetAuthorizedApplicationFormPG();
        Task<IEnumerable<ApplicationFormDto>> GetFinalizedApplicationFormPG();
        Task<IEnumerable<ApplicationFormRankingsDto>> GetApplicationFormsByAppliedCoursesPG(long? sessionId = null);
        Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudentsPG();
        //PG


        Task<ApplicationFormDto> GetApplicationForm(long Id);
        Task<ApplicationFormDto> GetApplicationFormByUserId(string UserId);
        Task DeleteApplicationForm(long Id);
         Task<ApplicationFormDto> GetBScApplicationFormById(long id);
        Task<ApplicationFormDto> UpdateApplicationForm(ApplicationFormDto ApplicationFormDto);
  
  Task<IEnumerable<ApplicationFormDto>> GetBScNotPaidApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScPaidNotSubmittedApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScSubmittedApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScDecisionMadeApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScProvisionalAdmissionApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScAcceptanceFeePaidApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScSchoolFeesPaidApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScHostelSelectionInProgressApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScHostelSelectedApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScMedicalFeePaidApplications();
    Task<IEnumerable<ApplicationFormDto>> GetBScMatriculationFeePaidApplications();
    Task<IEnumerable<ApplicationFormDto>> GetApplicationsStage11AndAbove();
    Task<IEnumerable<ApplicationFormDto>> GetApplicationsStage11AndAboves();
    Task<long> AddAdmittedStudent(AdmittedStudent admittedStudent);
        Task<IEnumerable<ApplicationFormDto>> GetApplicationsByMatriculationPrefix(string prefix);
        Task<bool> UpdateMatriculationNumber(long applicationId, string matriculationNumber);
        Task<IEnumerable<ApplicationFormDto>> GetRecentlyGeneratedMatriculationNumbers(int count = 10);
        Task<bool> IsApplicationEligibleForMatriculation(long applicationId);
        Task<IEnumerable<ApplicationFormDto>> GetPendingMatriculationApplications();
    Task<IEnumerable<ApplicationFormDto>> GetApplicationsWithMatriculationNumbers();

        // BSC Statistics
        Task<Dictionary<ApplicationStage, int>> GetBScApplicationStatistics();
    Task<int> GetBScApplicationCountByStage(ApplicationStage stage);
   
    Task<ApplicationFormDto> UpdateApplicationStage(ApplicationForm application);
/// <summary>
/// Gets all applications for a specific program name that have reached matriculation stage
/// Used for alphabetical sorting by last name when generating matriculation numbers
/// </summary>
/// <param name="programName">The program name to filter by</param>
/// <returns>A collection of application forms for the specified program</returns>
Task<IEnumerable<ApplicationFormDto>> GetApplicationsByProgramNameForMatriculation(string programName);

        
    }

}
