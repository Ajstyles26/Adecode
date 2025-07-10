using ACUnified.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models;

public class ApplicantPaymentLog : BaseClass
{
    //public long Id { get; set; }

    public decimal Amount { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? Comments { get; set; }

    public long? ApplicantPayCategoryId { get; set; }
    public virtual ApplicantPayCategory ApplicantPayCategory { get; set; }

    public long? ApplicantPayDetailsId { get; set; }
    public virtual ApplicantPayDetails ApplicantPayDetails { get; set; }

    public virtual Semester Semester { get; set; }
    public long? SemesterId { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? ReferenceNo { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? RRRRNo { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? UserId { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? StaffNo { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? email { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? tran_id { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? tran_type { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? tran_ref { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? pay_response { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? tran_status { get; set; }

    //vendor client code
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? client_code { get; set; }

    //client that made payment for the payee
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? client_name { get; set; }
    //payee full name
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? full_name { get; set; }
    //cocat paysetup full name session
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? tran_name { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string? policy { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public bool isSuccessful { get; set; }
    // public long ApplicationFormId { get; set; }
}