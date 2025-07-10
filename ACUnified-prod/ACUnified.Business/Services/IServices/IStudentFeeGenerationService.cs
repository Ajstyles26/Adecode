using ACUnified.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IStudentFeeGenerationService
    {
        Task GenerateFee(long categoryId);
        Task FinaliseFeeGeneration(long BatchId);
    }
}
