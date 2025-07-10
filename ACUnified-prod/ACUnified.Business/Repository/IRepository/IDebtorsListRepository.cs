using ACUnified.Data.DTOs;
namespace ACUnified.Business.Repository.IRepository
{
    public interface IDebtorsListRepository
    {
        Task<DebtorsListDto> CreateDebtorsList(DebtorsListDto DebtorsListDto);
        Task<IEnumerable<DebtorsListDto>> GetAllDebtorsList();
        Task<DebtorsListDto> GetDebtorsList(long Id);
        Task DeleteDebtorsList(long Id);
        Task<DebtorsListDto> UpdateDebtorsList(DebtorsListDto DebtorsListDto);
    }

}
