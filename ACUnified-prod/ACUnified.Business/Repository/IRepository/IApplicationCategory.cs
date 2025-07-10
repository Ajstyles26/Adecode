using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IApplicationCategoryRepository
    {
        Task<ApplicantCategoryDto> CreateApplicantCategory(ApplicantCategoryDto ApplicantCategoryDto);
        Task<IEnumerable<ApplicantCategoryDto>> GetAllApplicantCategory();
        Task<ApplicantCategoryDto> GetApplicantCategory(long Id);
        Task DeleteApplicantCategory(long Id);
        Task<ApplicantCategoryDto> UpdateApplicantCategory(ApplicantCategoryDto ApplicantCategoryDto);
    }

}
