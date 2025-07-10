using System;
using System.Collections.Generic;
using System.IO;
using System.Text;  // Add this line
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Business.IServices;
using ACUnified.Business.Services.IServices;
using ACUnified.Business.Services;

namespace ACUnified.Business.Services
{
    public class HostelDebtorListExportService : IHostelDebtorListExportService
    {
        public async Task<byte[]> ExportToCsvAsync(IEnumerable<HostelDebtorListDto> data, string statusFilter = "all", string searchText = "")
        {
            var filteredData = FilterData(data, statusFilter, searchText);

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = args => true
            });

            // Configure CSV mapping
            csv.Context.RegisterClassMap<HostelDebtorListMap>();

            // Write records
            await csv.WriteRecordsAsync(filteredData);
            await writer.FlushAsync();
            return memoryStream.ToArray();
        }

        public string GetExportFileName()
        {
            return $"applicant_debtor_list_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        }

        private IEnumerable<HostelDebtorListDto> FilterData(
            IEnumerable<HostelDebtorListDto> data,
            string statusFilter,
            string searchText)
        {
            var query = data;

            // Apply status filter
            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                query = statusFilter switch
                {
                    "outstanding" => query.Where(x => x.OutstandingAmount > 0),
                    "paid" => query.Where(x => x.OutstandingAmount <= 0),
                    "overdue" => query.Where(x => x.DueDate < DateTime.Today && x.OutstandingAmount > 0),
                    "firstInstalment" => query.Where(x => x.PaymentPlan == PayPlan.FirstInstalment),
                    "secondInstalment" => query.Where(x => x.PaymentPlan == PayPlan.SecondInstalment),
                    _ => query
                };
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.ToLower();
                query = query.Where(x =>
                    (GetApplicantFullName(x).ToLower().Contains(searchText)) ||
                    (x.ApplicationForm?.FinalisedCourse ?? "").ToLower().Contains(searchText) ||
                    x.ApplicationFormId.ToString().Contains(searchText) ||
                    (x.ApplicationForm?.Level?.Name ?? "").ToLower().Contains(searchText) ||
            (x.ApplicationForm?.MatriculationNumber ?? "").ToLower().Contains(searchText)); // Added matriculation number filter
    }

            return query;
        }

        private string GetApplicantFullName(HostelDebtorListDto record)
        {
            if (record.ApplicationForm?.BioData == null)
                return "N/A";

            return $"{record.ApplicationForm.BioData.LastName} {record.ApplicationForm.BioData.FirstName} {record.ApplicationForm.BioData.OtherName}".Trim();
        }
    }

    public sealed class HostelDebtorListMap : ClassMap<HostelDebtorListDto>
    {
        public HostelDebtorListMap()
        {
            Map(m => m.Id).Name("ID");
            Map(m => m.ApplicationForm).Name("Application ID");
            Map(m => m.ApplicationForm.BioData).Convert(row =>
            {
                var record = row.Value;
                if (record.ApplicationForm?.BioData == null)
                    return "N/A";
                return $"{record.ApplicationForm.BioData.LastName} {record.ApplicationForm.BioData.FirstName} {record.ApplicationForm.BioData.OtherName}".Trim();
            }).Name("Applicant Name");
            Map(m => m.ApplicationForm.FinalisedCourse).Convert(row =>
                row.Value.ApplicationForm?.FinalisedCourse ?? "N/A").Name("Course");
            Map(m => m.ApplicationForm.Level.Name).Convert(row =>
                row.Value.ApplicationForm?.Level?.Name ?? "N/A").Name("Level");

            // Add matriculation number mapping
            Map(m => m.ApplicationForm.MatriculationNumber).Convert(row =>
                row.Value.ApplicationForm?.MatriculationNumber ?? "N/A").Name("Matriculation Number");

            Map(m => m.TotalAmount).Name("Total Amount");
            Map(m => m.TotalPaid).Name("Total Paid");
            Map(m => m.OutstandingAmount).Name("Outstanding Amount");
            Map(m => m.DueDate).Convert(row =>
                row.Value.DueDate.ToShortDateString()).Name("Due Date");
            Map(m => m.PaymentPlan).Convert(row =>
            {
                if (row.Value.OutstandingAmount <= 0)
                    return "Fully Paid";
                return row.Value.PaymentPlan switch
                {
                    PayPlan.FirstInstalment => "First Instalment",
                    PayPlan.SecondInstalment => "Second Instalment",
                    PayPlan.Full => "Full Payment Required",
                    _ => "Unknown"
                };
            }).Name("Payment Status");
        }
    }
}