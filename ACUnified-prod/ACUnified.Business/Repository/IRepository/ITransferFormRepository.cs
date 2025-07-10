using ACUnified.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository.IRepository
{
    public interface ITransferFormRepository
    {
        Task<TransferFormDto> CreateTransferForm(TransferFormDto transferForm);
        Task<IEnumerable<TransferFormDto>> GetAllTransferForm();
        Task<TransferFormDto> GetTransferForm(string userId);
        Task<TransferFormDto> UpdateTransferForm(TransferFormDto transferForm);
        Task DeleteTransferForm(long Id);
    }
}