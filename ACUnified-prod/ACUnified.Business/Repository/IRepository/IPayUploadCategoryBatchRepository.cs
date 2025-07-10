using ACUnified.Data.DTOs;
using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Repository.IRepository
{
    public interface IPayUploadCategoryBatchRepository
    {
        Task<PayUploadCategoryBatchDto> CreatePayUploadCategoryBatch(PayUploadCategoryBatchDto PayUploadCategoryBatchDto);
        Task CreatePayUploadCategoryBatch(IEnumerable<PayUploadCategoryBatchDto> PayUploadCategoryDto);
        Task<IEnumerable<PayUploadCategoryBatchDto>> GetAllPayUploadCategoryBatch();
        Task<PayUploadCategoryBatchDto> GetPayUploadCategoryBatch(long Id);
        Task DeletePayUploadCategoryBatch(long Id);
        Task<PayUploadCategoryBatchDto> UpdatePayUploadCategoryBatch(PayUploadCategoryBatchDto PayUploadCategoryDto);

    }
}
