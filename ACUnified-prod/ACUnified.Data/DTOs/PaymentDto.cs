using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.DTOs;

public class PaymentDto
{
    public long Id { get; set; }
    
    public decimal Amount { get; set; }


    public string? Comments { get; set; }

    public long? PayCategoryId { get; set; }
    public virtual PayCategory? PayCategory { get; set; }

    public long? PayDetailsId { get; set; }
    public virtual PayDetails? PayDetails { get; set; }

    public virtual Semester Semester { get; set; }
    public long? SemesterId { get; set; }

    public long? SessionId { get; set; }

    public virtual Session Session { get; set; }

    //this is updated whan user picks the pay plan
    public PayPlan? ProformaPayPlan { get; set; }
        

    // public long? PaymentChannelSetupId { get; set; }
    // public PaySetup.PayChannelSetup PaymentChannelSetup { get; set; }

    public string? ReferenceNo { get; set; }

    public string? RRRRNo { get; set; }
    public string? MatricNo { get; set; }

    public string? StaffNo { get; set; }


    public string? tran_id { get; set; }

    public string? tran_type { get; set; }

    public string? tran_ref { get; set; }

    public string? pay_response { get; set; }

    public string? tran_status { get; set; }

    //client that made payment for the payee
    public string client_name { get; set; }
    //payee full name
    public string full_name { get; set; }
    //cocat paysetup full name session
    public string tran_name { get; set; }

    
    public bool isSuccessful { get; set; }
    
    public DateTime CreatedDate { get; set; } =DateTime.Now;

    public string? AdmissionNo { get; set; }

    public string? OldMatricNo { get; set; }
}