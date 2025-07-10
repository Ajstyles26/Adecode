using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;
using ACUnified.Data.Models;


namespace ACUnified.Business.Repository.IRepository;

    public interface IApplicantDebtorListRepository
    {
        Task<IEnumerable<ApplicantDebtorListDto>> GetAllAsync();
        //    Task<List<ApplicantDebtorListDto>> GetAllAsync();
        Task<ApplicantDebtorListDto> GetByIdAsync(long id);
        Task<ApplicantDebtorListDto> CreateAsync(ApplicantDebtorList debtorList);
        Task<ApplicantDebtorListDto> UpdateAsync(ApplicantDebtorList debtorList);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<ApplicantDebtorListDto>> GetByApplicationFormIdAsync(long applicationFormId);
        Task<bool> CheckPaymentExistsAsync(long applicationFormId);
        Task<bool> IsValidPaymentCategory(long applicationFormId);
    }