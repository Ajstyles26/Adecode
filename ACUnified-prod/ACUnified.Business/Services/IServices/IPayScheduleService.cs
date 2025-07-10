using ACUnified.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IPayScheduleService
    {
        Task<IEnumerable<PayUploadCategoryDto>> ProcessCSV(byte[] fileBytes);
        Task<IEnumerable<PayUploadCategoryDto>> ProcessCSVWithTimeout(byte[] fileBytes, TimeSpan timeout);
    }
}
