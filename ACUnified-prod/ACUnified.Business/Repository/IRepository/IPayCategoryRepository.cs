using ACUnified.Data.Enum;
using ACUnified.Data.DTOs;

public interface IPayCategoryRepository
{
    Task<PayCategoryDto> CreatePayCategory(PayCategoryDto PayCategoryDto);

    Task CreatePayCategory(IEnumerable<PayCategoryDto> PayCategoryDto);
    Task<IEnumerable<PayCategoryDto>> GetAllPayCategory();
    Task<PayCategoryDto> GetPayCategory(long Id);
    Task DeletePayCategory(long Id);
    Task<PayCategoryDto> UpdatePayCategory(PayCategoryDto PayCategoryDto);

    Task<IEnumerable<PayCategoryDto>> GetStudentPayCategory(long LevelId, long Department,
        long SessionId,long DegreeId);

}
