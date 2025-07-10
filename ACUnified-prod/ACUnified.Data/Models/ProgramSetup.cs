using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models
{
    public class ProgramSetup : BaseClass
    {
        // public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(80)]
        public string Name { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Description { get; set; }
        [ForeignKey("Faculty")]
        public long FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
        [ForeignKey("Department")]
        public long DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [ForeignKey("Degree")]
        public long? DegreeId { get; set; }
        public virtual Degree? Degree { get; set; }

        public int CourseRegMinLimit { get; set; }
        public int CourseRegMaxLimit { get; set; }

        public int Quota { get; set; }

        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }

    }
}

