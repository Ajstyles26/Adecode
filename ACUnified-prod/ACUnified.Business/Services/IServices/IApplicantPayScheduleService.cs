using ACUnified.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IApplicantPayScheduleService
    {
        Task<IEnumerable<ApplicantPayUploadCategoryDto>> ProcessCSV(byte[] fileBytes);
        Task<IEnumerable<ApplicantPayUploadCategoryDto>> ProcessCSVWithTimeout(byte[] fileBytes, TimeSpan timeout);
    }
}
