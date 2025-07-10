using ACUnified.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IApplicantPaymentMigrationService
    {
        Task<List<ApplicantDebtorListDto>> MigratePaymentsToDebtorListAsync(
            int startId = 9, 
            int endId = 53, 
            Action<int> progressCallback = null);
    }
}