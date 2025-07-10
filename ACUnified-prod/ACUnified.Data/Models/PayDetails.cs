using ACUnified.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models;

public class PayDetails : BaseClass
{
    // public long Id { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string Name { get; set; }

    public decimal OriginalAmount { get; set; } = 0;
    /// <summary>
    /// original minus concession amount is total amount due for the semester
    /// </summary>
    public decimal ConcessionAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public decimal PayInstalment1 { get; set; } = 0;
    public decimal PayInstalment2 { get; set; } = 0;
    public decimal PayInstalment3 { get; set; } = 0;
    //second and third payment fee
    public decimal PayInstalment4 { get; set; } = 0;


    public long? StudentId { get; set; }

    public virtual Student? Student { get; set; }

    public long? PayCategoryId { get; set; }

    public virtual PayCategory? PayCategory { get; set; }

    public long? PayUploadCategoryBatchId { get; set; }

    public virtual PayUploadCategoryBatch? PayUploadCategoryBatch { get; set; }

    public long? PayConcessionId { get; set; }
    public virtual PayConcession? PayConcession { get; set; }

    public bool IsForCourseRegistration { get; set; }

    public bool PartAllowed { get; set; } = true;

    public bool isLateFee { get; set; } = false;

    public long? SessionId { get; set; }
    public virtual Session? Session { get; set; }
     
    public long? DegreeId { get; set; }
    public virtual Degree? Degree { get; set; }

}