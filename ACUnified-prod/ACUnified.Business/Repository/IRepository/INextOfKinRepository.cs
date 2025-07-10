using ACUnified.Data.DTOs;


public interface INextOfKinRepository
{
    Task<NextOfKinDto> CreateNextOfKin(NextOfKinDto nextofkinDto);
     
    public  Task<NextOfKinDto> GetNextOfKin(string userid);
    Task<IEnumerable<NextOfKinDto>> GetAllNextOfKin();
    Task DeleteNextOfKin(long Id);
    Task<NextOfKinDto> UpdateNextOfKin(NextOfKinDto nextofkinDto);
}
