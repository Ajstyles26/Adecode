using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class RemitaNotificationDto
    {
            public long Id { get; set; }
            public string? rrr { get; set; }
            public string? billerName { get; set; }
            public string? channel { get; set; }
            public double? amount { get; set; }
            public DateTime? transactiondate { get; set; }
            public DateTime? debitdate { get; set; }
            public string? bank { get; set; }
            public string? branch { get; set; }
            public string? serviceTypeId { get; set; }
            public string? orderRef { get; set; }
            public string? orderId { get; set; }
            public string? payerName { get; set; }
            public string? payerPhoneNumber { get; set; }
            public string? payerEmail { get; set; }
            public string? type { get; set; }
            public double? chargeFee { get; set; }
            public string? paymentDescription { get; set; }
            public string? integratorsEmail { get; set; }
            public string? integratorsPhonenumber { get; set; }
        
    }
}
