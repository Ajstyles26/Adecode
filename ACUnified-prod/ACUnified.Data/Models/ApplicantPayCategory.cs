using ACUnified.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models;

public class ApplicantPayCategory : BaseClass
{
    //public long Id { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string Name { get; set; }

    public decimal? TotalPayDue { get; set; } = 0;

    public decimal? PayInstalment1 { get; set; } = 0;
    public decimal? PayInstalment2 { get; set; } = 0;
    public decimal? PayInstalment3 { get; set; } = 0;
    //second and third payment fee
    public decimal? PayInstalment4 { get; set; } = 0;

    public long? StudentLevelId { get; set; }

    public virtual Level? StudentLevel { get; set; }

    public long? SessionId { get; set; }
    public virtual Session? Session { get; set; }

    public virtual Semester? Semester { get; set; }
    public long? SemesterId { get; set; }

    public bool IsInstallmental { get; set; } = false;

   

    public long? DepartmentId { get; set; }
    public virtual Department? Department { get; set; }



    public long? DegreeId { get; set; }
    public Degree? Degree { get; set; }

    //institution provider
    //public virtual long? InstitutionProviderId { get; set; }
    //public virtual InstitutionProvider? InstitutionProvider { get; set; }

}

