using System.Collections.Generic;
using ACUnified.Data.DTOs;

namespace ACUnified.Business.IServices
{
    public interface ICsvExportServices
    {
        byte[] ExportToCsvs(IEnumerable<ApplicationFormDto> data);
    }
}
