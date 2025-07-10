using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.DTOs;
using ACUnified.Data.Payloads;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Text;
using System.Text.Json;

namespace ACUnified.Portal.Controllers
{
    public class MigrateController : ControllerBase
{
    private readonly IApplicantPaymentMigrationService _migrationService;

    public MigrateController(IApplicantPaymentMigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    public async Task<IActionResult> MigratePayments()
    {
        var results = await _migrationService.MigratePaymentsToDebtorListAsync();
        return Ok(results);
    }
}
}