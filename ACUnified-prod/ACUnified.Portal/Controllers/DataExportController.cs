using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace ACUnified.Portal.Controllers
{
    public class DataExportController : Controller
    {
        [HttpPost]
        private IActionResult ExportToExcel<T>(IEnumerable<T> items)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // Write headers
                var headers = typeof(T).GetProperties();
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i].Name;
                }

                // Write data
                int row = 2;
                foreach (var item in items)
                {
                    var properties = item.GetType().GetProperties();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cells[row, i + 1].Value = properties[i].GetValue(item);
                    }
                    row++;
                }

                var memoryStream = new MemoryStream(excelPackage.GetAsByteArray());
                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "table.xlsx");
            }
        }
    }
}
