using ACUnified.Data.DTOs;
using ACUnified.Data.Models;

public interface IPayUploadConcessionRepository
{
    Task<PayUploadConcessionDto> CreatePayUploadConcession(PayUploadConcessionDto PayUploadConcessionDto);
    Task<IEnumerable<PayUploadConcessionDto>> GetAllPayUploadConcession();
    Task<PayUploadConcessionDto> GetPayUploadConcession(long Id);
    Task<IEnumerable<PayUploadConcessionDto>> GetPayUploadConcessionBatch(long Id);
    Task DeletePayUploadConcession(long Id);
    Task<PayUploadConcessionDto> UpdatePayUploadConcession(PayUploadConcessionDto PayUploadConcessionDto);
}
