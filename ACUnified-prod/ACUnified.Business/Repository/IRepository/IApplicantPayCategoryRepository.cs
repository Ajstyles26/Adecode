using ACUnified.Data.DTOs;


public interface IApplicantPayCategoryRepository
{
    Task<ApplicantPayCategoryDto> CreateApplicantPayCategory(ApplicantPayCategoryDto PayCategoryDto);
    Task<IEnumerable<ApplicantPayCategoryDto>> GetAllApplicantPayCategory();
    Task<ApplicantPayCategoryDto> GetApplicantPayCategory(long Id);
    Task DeleteApplicantPayCategory(long Id);
    Task<ApplicantPayCategoryDto> UpdateApplicantPayCategory(ApplicantPayCategoryDto ApplicantPayCategoryDto);

    Task<IEnumerable<ApplicantPayCategoryDto>> GetStudentApplicantPayCategory(long LevelId, long Department,
        long SessionId,long DegreeId);

}
