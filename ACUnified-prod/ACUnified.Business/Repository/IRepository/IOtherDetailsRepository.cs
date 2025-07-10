using ACUnified.Data.DTOs;


public interface IOtherDetailsRepository
{
    Task<OtherDetailsDto> CreateOtherDetails(OtherDetailsDto otherdetailsDto);
     
    public  Task<OtherDetailsDto> GetOtherDetails(string userid);
    Task<IEnumerable<OtherDetailsDto>> GetAllOtherDetails();
    Task DeleteOtherDetails(long Id);
    Task<OtherDetailsDto> UpdateOtherDetails(OtherDetailsDto otherdetailsDto);
}
