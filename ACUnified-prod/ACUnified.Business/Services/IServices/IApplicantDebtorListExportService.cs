using System.Collections.Generic;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;

namespace ACUnified.Business.Services.IServices
{
    public interface IApplicantDebtorListExportService
    {
        Task<byte[]> ExportToCsvAsync(IEnumerable<ApplicantDebtorListDto> data, string statusFilter = "all", string searchText = "");
        string GetExportFileName();
    }
}