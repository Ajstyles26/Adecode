using ACUnified.Data.Enum;
using ACUnified.Data.DTOs;

public interface IPayUploadDetailsRepository
{
    Task<PayUploadDetailsDto> CreatePayUploadDetails(PayUploadDetailsDto PayUploadDetailsDto);
    Task CreatePayUploadDetails(IEnumerable<PayUploadDetailsDto> payUploadDetailsDto);
    Task<IEnumerable<PayUploadDetailsDto>> GetAllPayUploadDetails();
    Task<PayUploadDetailsDto> GetPayUploadDetails(long Id);
    Task<PayUploadDetailsDto> GetPayUploadDetailsByName(string Name);
    Task DeletePayUploadDetails(long Id);
    Task DeletePayUploadDetailsByCategoryBatch(long categoryBatchId);
    Task<PayUploadDetailsDto> UpdatePayUploadDetails(PayUploadDetailsDto PayUploadDetailsDto);
    Task<IEnumerable<PayUploadDetailsDto>> GetStudentPayUploadDetails(SemesterType currentSemester, long categoryId, bool isSemesterLate);
    Task<IEnumerable<PayUploadDetailsDto>> GetPayUploadDetailsBatch(long Id);
}
