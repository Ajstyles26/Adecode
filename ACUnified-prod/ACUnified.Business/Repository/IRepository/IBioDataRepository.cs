


using ACUnified.Data.DTOs;

public interface IBioDataRepository
{
    Task<BioDataDto> CreateBioData(BioDataDto biodataDto);
     
    public  Task<BioDataDto> GetBioData(string userid);
    Task<IEnumerable<BioDataDto>> GetAllBioData();
    Task DeleteBioData(long Id);
    Task<BioDataDto> UpdateBioData(BioDataDto biodataDto);
}
