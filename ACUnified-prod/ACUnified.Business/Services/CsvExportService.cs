using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ACUnified.Data.DTOs;
using ACUnified.Business.IServices;
using ACUnified.Business.Services.IServices;
using ACUnified.Business.Services;

namespace ACUnified.Business.Services
{
     public class CsvExportService : ICsvExportService
    {
        public byte[] ExportToCsv(IEnumerable<ApplicationFormDto> data)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                // Write headers
                csvWriter.WriteField("Form Ref No");
                csvWriter.WriteField("First Name");
                csvWriter.WriteField("Last Name");
                csvWriter.WriteField("Finalised Course");
                csvWriter.WriteField("Gender");
                csvWriter.WriteField("State");
                csvWriter.WriteField("Mobile Number");
                csvWriter.WriteField("Email");
                csvWriter.NextRecord();

                // Write data
                foreach (var item in data)
                {
                    csvWriter.WriteField(item.FormRefNo);
                    csvWriter.WriteField(item.BioData?.FirstName);
                    csvWriter.WriteField(item.BioData?.LastName);
                    csvWriter.WriteField(item.FinalisedCourse);
                    csvWriter.WriteField(item.BioData?.Gender);
                    csvWriter.WriteField(item.BioData?.State);
                    csvWriter.WriteField(item.BioData?.MobileNumber);
                    csvWriter.WriteField(item.BioData?.Email);
                    csvWriter.NextRecord();
                }

                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }
    }
}