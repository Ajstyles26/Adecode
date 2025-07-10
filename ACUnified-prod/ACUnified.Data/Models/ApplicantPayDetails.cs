using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACUnified.Data.Enum;

namespace ACUnified.Data.Models;

public class ApplicantPayDetails : BaseClass
{
    // public long Id { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string Name { get; set; }


    public decimal Amount { get; set; }

    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string Description { get; set; }


    public long? ApplicantPayCategoryId { get; set; }

    public virtual ApplicantPayCategory? ApplicantPayCategory { get; set; }

    public bool isForCourseRegistration { get; set; }

      public long? ProgramSetupId { get; set; }
        public virtual ProgramSetup? ProgramSetup { get; set; }
    //The payment is for which Semester
    public SemesterType Semester { get; set; } = SemesterType.First;

    public long? SessionId { get; set; }
    public Session? Session { get; set; }

    public bool partAllowed { get; set; }
}