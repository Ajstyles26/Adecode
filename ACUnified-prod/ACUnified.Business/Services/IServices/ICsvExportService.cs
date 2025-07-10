using System.Collections.Generic;
using ACUnified.Data.DTOs;

namespace ACUnified.Business.IServices
{
    public interface ICsvExportService
    {
        byte[] ExportToCsv(IEnumerable<ApplicationFormDto> data);
    }
}

