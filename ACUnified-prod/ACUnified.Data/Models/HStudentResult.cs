using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ACUnified.Data.Models
{
    public class HStudentResult : BaseClass
    {

        //public long Id { get; set; }
        public decimal SemesterGPA { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string MatricNo { get; set; }

        [ForeignKey("Student")]
        public long StudentId { get; set; }
        public virtual Student Student { get; set; }
        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }

    }
}

