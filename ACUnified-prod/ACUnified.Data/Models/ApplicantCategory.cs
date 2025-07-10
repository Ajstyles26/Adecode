using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models;

public class ApplicantCategory:BaseClass
{
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string ProgramName { get; set; }
    public decimal Amount { get; set; }
    //institution provider
    public virtual long? InstitutionProviderId { get; set; }
    public virtual InstitutionProvider? InstitutionProvider { get; set; }
}