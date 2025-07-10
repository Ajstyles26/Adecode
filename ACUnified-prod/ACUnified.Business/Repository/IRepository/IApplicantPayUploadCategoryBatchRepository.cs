using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IApplicantPayUploadCategoryBatchRepository
    {
        Task<ApplicantPayUploadCategoryBatchDto> CreatePayUploadCategoryBatch(ApplicantPayUploadCategoryBatchDto PayUploadCategoryBatchDto);
        Task CreatePayUploadCategoryBatch(IEnumerable<ApplicantPayUploadCategoryBatchDto> PayUploadCategoryDto);
        Task<IEnumerable<ApplicantPayUploadCategoryBatchDto>> GetAllPayUploadCategoryBatch();
        Task<ApplicantPayUploadCategoryBatchDto> GetPayUploadCategoryBatch(long Id);
        Task DeletePayUploadCategoryBatch(long Id);
        Task<ApplicantPayUploadCategoryBatchDto> UpdatePayUploadCategoryBatch(ApplicantPayUploadCategoryBatchDto PayUploadCategoryDto);

    }
}
