using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IApplicantPayUploadCategoryRepository
    {
        Task<ApplicantPayUploadCategoryDto> CreatePayUploadCategory(ApplicantPayUploadCategoryDto ApplicantPayUploadCategoryDto);
        Task CreatePayUploadCategory(IEnumerable<ApplicantPayUploadCategoryDto> ApplicantPayUploadCategoryDto);
        Task<IEnumerable<ApplicantPayUploadCategoryDto>> GetPayUploadCategoryByBatchId(long id);
        Task<IEnumerable<ApplicantPayUploadCategoryDto>> GetPayUploadCategoryByBDPL(long batchid,long degreeid, long programid,long levelid);
        Task<IEnumerable<ApplicantPayUploadCategoryDto>> GetAllPayUploadCategory();
        Task<ApplicantPayUploadCategoryDto> GetPayUploadCategory(long Id);
        Task DeletePayUploadCategory(long Id);
        Task<ApplicantPayUploadCategoryDto> UpdatePayUploadCategory(ApplicantPayUploadCategoryDto ApplicantPayUploadCategoryDto);
        Task<IEnumerable<ApplicantPayUploadCategoryDto>> GetPayUploadCategoryBatch(long Id);

    }
}
