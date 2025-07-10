using ACUnified.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models;

public class PaymentLog : BaseClass
{
    //public long Id { get; set; }

    public decimal Amount { get; set; } = 0.00m;

    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public string? Comments { get; set; }

    public long? PayCategoryId { get; set; }
    public virtual PayCategory? PayCategory { get; set; }

    public long? PayDetailsId { get; set; }
    public virtual PayDetails? PayDetails { get; set; }

    public virtual Semester? Semester { get; set; }
    public long? SemesterId { get; set; }

    public virtual Session? Session { get; set; }
    public long? SessionId { get; set; }

    // public long? PaymentChannelSetupId { get; set; }
    // public PaySetup.PayChannelSetup PaymentChannelSetup { get; set; }
    //this is updated whan user picks the pay plan
    public PayPlan? ProformaPayPlan { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public string? ReferenceNo { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? RRRRNo { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string? MatricNo { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string? ReceiptNo { get; set; }

  

    [Column(TypeName = "VARCHAR")]
    [StringLength(20)]
    public string? StaffNo { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? tran_id { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? tran_type { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public string? tran_ref { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? pay_response { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? tran_status { get; set; }

    //client that made payment for the payee
    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public string? client_name { get; set; }
    //payee full name
    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public string? full_name { get; set; }
    //cocat paysetup full name session
    [Column(TypeName = "VARCHAR")]
    [StringLength(160)]
    public string? tran_name { get; set; }

    public bool isSuccessful { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public string? AdmissionNo { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(150)]
    public string? OldMatricNo { get; set; }
}