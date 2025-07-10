using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services
{
    public class PayScheduleService : IPayScheduleService
    {
        IPayUploadCategoryBatchRepository _PUCBRepository;
        IPayUploadCategoryRepository _PUCRepository;
        public PayScheduleService(IPayUploadCategoryBatchRepository PUCBRepository, IPayUploadCategoryRepository pUCRepository)
        {
            _PUCBRepository = PUCBRepository;
            _PUCRepository = pUCRepository;

        }
        public async Task<IEnumerable<PayUploadCategoryDto>> ProcessCSV(byte[] fileBytes)
        {
            var paymentSchedules = new List<PayUploadCategoryDto>();

            using (var stream = new MemoryStream(fileBytes))
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return paymentSchedules;

                var totalRows = worksheet.Dimension.Rows;
                //create your batch 
                PayUploadCategoryBatchDto currentPUCBDto = new PayUploadCategoryBatchDto();
                currentPUCBDto.Name = $"Schedule Batch {DateTime.UtcNow}";
                currentPUCBDto.FeeUploadStatus = ProcessStatus.Pending;
                currentPUCBDto.FeeGenerationStatus = ProcessStatus.Pending;
                var svdcurrentPUCBDto=await _PUCBRepository.CreatePayUploadCategoryBatch(currentPUCBDto);
                for (int row = 2; row <= totalRows; row++)
                {
                    var paymentSchedule = new PayUploadCategoryDto
                    {
                        Name = worksheet.Cells[row, 2].Value.ToString(),

                        StudentLevelId = Int64.Parse(worksheet.Cells[row, 5].Value.ToString()),
                        TotalPayDue = decimal.Parse(worksheet.Cells[row, 3].Value.ToString()),
                        IsGlobal = bool.Parse(worksheet.Cells[row, 4].Value.ToString()),

                        SessionId = Int64.Parse(worksheet.Cells[row, 6].Value.ToString()),
                        DegreeId = Int64.Parse(worksheet.Cells[row, 7].Value.ToString()),
                        entryMode = (EntryMode)int.Parse(worksheet.Cells[row, 8].Value.ToString()),

                        ProgramSetupId = Int64.Parse(worksheet.Cells[row, 9].Value.ToString()),
                        PayInstalment1 = CalculateInstallments(decimal.Parse(worksheet.Cells[row, 3].Value.ToString()), 50),
                        PayInstalment2 = CalculateInstallments(decimal.Parse(worksheet.Cells[row, 3].Value.ToString()), 25),
                        PayInstalment3 = CalculateInstallments(decimal.Parse(worksheet.Cells[row, 3].Value.ToString()), 25),
                        PayInstalment4 = CalculateInstallments(decimal.Parse(worksheet.Cells[row, 3].Value.ToString()), 50),
                        PayUploadCategoryBatchId = svdcurrentPUCBDto.Id
                    };
                    Console.WriteLine("Information is " + worksheet.Cells[row,4].Value.ToString());
                    Console.WriteLine("Information is " + worksheet.Cells[row, 2].Value.ToString());

                    paymentSchedules.Add(paymentSchedule);
                }
            }
            await _PUCRepository.CreatePayUploadCategory(paymentSchedules);
            return paymentSchedules;
        }


        public async Task<IEnumerable<PayUploadCategoryDto>> ProcessCSVWithTimeout(byte[] fileBytes, TimeSpan timeout)
        {
            var paymentSchedules = new List<PayUploadCategoryDto>();

            // Create a cancellation token source with timeout
            using var cancellationTokenSource = new CancellationTokenSource(timeout);
            var cancellationToken = cancellationTokenSource.Token;

            // Execute the ProcessCSV method within a separate task
            var task = Task.Run(() => ProcessCSV(fileBytes), cancellationToken);

            try
            {
                // Wait for either the task to complete or timeout
                await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cancellationToken));

                // If the task completed successfully, return the result
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    return task.Result;
                }
                else
                {
                    // Handle task failure or cancellation
                    throw new OperationCanceledException("CSV processing operation timed out.");
                }
            }
            catch (OperationCanceledException)
            {
                // Handle timeout scenario
                throw new OperationCanceledException("CSV processing operation timed out.");
            }
        }
        public decimal CalculateInstallments(decimal totalPayDue, int InstalmentQuota)
        {
            // Implement your installment calculation logic here
            // For example, you could split the total payment into three equal installments
            if (totalPayDue<=0 || InstalmentQuota <= 0) {
                return 1;
            }

            decimal installment = (totalPayDue)*((decimal)InstalmentQuota/100m);


            return installment;
        }
    }
}
