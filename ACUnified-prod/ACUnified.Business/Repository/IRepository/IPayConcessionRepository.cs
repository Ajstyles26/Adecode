using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

public interface IPayConcessionRepository
{
    Task<PayConcessionDto> CreatePayConcession(PayConcessionDto PayConcessionDto);
    Task CreatePayConcession(IEnumerable<PayConcessionDto> PayConcessionDto);
    Task<IEnumerable<PayConcessionDto>> GetAllPayConcession();
    Task<PayConcessionDto> GetPayConcession(long Id);
    Task DeletePayConcession(long Id);
    Task<PayConcessionDto> UpdatePayConcession(PayConcessionDto PayConcessionDto);
    Task<IEnumerable<PayConcessionDto>> GetAllPayByUserIdConcession(string Userid);


}
