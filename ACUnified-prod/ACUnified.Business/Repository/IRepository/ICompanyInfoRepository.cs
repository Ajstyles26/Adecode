

using ACUnified.Data.DTOs;

public interface ICompanyInfoRepository
{
    Task<CompanyInfoDto> CreateCompanyInfo(CompanyInfoDto companyinfoDto);
    Task<IEnumerable<CompanyInfoDto>> GetAllCompanyInfo();
    Task<CompanyInfoDto> GetCompanyInfo(long Id);
    Task DeleteCompanyInfo(long Id);
    Task<CompanyInfoDto> UpdateCompanyInfo(CompanyInfoDto companyinfoDto);
}
