using System.Collections.Generic;
using System.Threading.Tasks;
using ACUnified.Data.DTOs;

namespace ACUnified.Business.Services.IServices
{
    public interface IHostelDebtorListExportService
    {
        Task<byte[]> ExportToCsvAsync(IEnumerable<HostelDebtorListDto> data, string statusFilter = "all", string searchText = "");
        string GetExportFileName();
    }
}