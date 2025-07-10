using ACUnified.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IHostelPaymentMigrationService
    {
        Task<List<HostelDebtorListDto>> MigratePaymentsToDebtorListAsync(
            int startId = 60,
            Action<int> progressCallback = null);
    }
}