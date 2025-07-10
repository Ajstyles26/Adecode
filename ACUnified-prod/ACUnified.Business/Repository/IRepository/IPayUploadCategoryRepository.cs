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
    public interface IPayUploadCategoryRepository
    {
        Task<PayUploadCategoryDto> CreatePayUploadCategory(PayUploadCategoryDto PayUploadCategoryDto);
        Task CreatePayUploadCategory(IEnumerable<PayUploadCategoryDto> PayUploadCategoryDto);
        Task<IEnumerable<PayUploadCategoryDto>> GetPayUploadCategoryByBatchId(long id);
        Task<IEnumerable<PayUploadCategoryDto>> GetPayUploadCategoryByBDPL(long batchid,long degreeid, long programid,long levelid);
        Task<IEnumerable<PayUploadCategoryDto>> GetAllPayUploadCategory();
        Task<PayUploadCategoryDto> GetPayUploadCategory(long Id);
        Task DeletePayUploadCategory(long Id);
        Task<PayUploadCategoryDto> UpdatePayUploadCategory(PayUploadCategoryDto PayUploadCategoryDto);
        Task<IEnumerable<PayUploadCategoryDto>> GetPayUploadCategoryBatch(long Id);

    }
}
