using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;


namespace ACUnified.Business.Repository.IRepository;

    public interface IHostelDebtorListRepository
    {
        Task<IEnumerable<HostelDebtorListDto>> GetAllAsync();
        //    Task<List<HostelDebtorListDto>> GetAllAsync();
        Task<HostelDebtorListDto> GetByIdAsync(long id);
        Task<HostelDebtorListDto> CreateAsync(HostelDebtorList debtorList);
        Task<HostelDebtorListDto> UpdateAsync(HostelDebtorList debtorList);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<HostelDebtorListDto>> GetByApplicationFormIdAsync(long applicationFormId);
        Task<bool> CheckPaymentExistsAsync(long applicationFormId);
        Task<bool> IsValidPaymentCategory(long applicationFormId);
    }