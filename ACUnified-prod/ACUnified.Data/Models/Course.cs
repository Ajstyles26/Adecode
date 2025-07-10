using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACUnified.Data.Enum;

namespace ACUnified.Data.Models
{
    public class Course : BaseClass
    {
        //public long Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CourseCode { get; set; }

        public int CourseUnit { get; set; }

        public SemesterType? Semester { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Description { get; set; }
        
        public CourseCategory courseCategory { get; set; }
        //public long? FacultyId { get; set; }
        //public virtual Faculty? Faculty { get; set; }
        public long DegreeId { get; set; }
        public virtual Degree Degree { get; set; }
        public long? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        //public long? ProgramSetupId { get; set; }
        //public virtual ProgramSetup? ProgramSetup { get; set; }
        public long? LevelId { get; set; }

        public virtual Level? Level { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Status { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Prerequisite { get; set; }
        
        //institution provider
        public virtual long? InstitutionProviderId{get;set;}
        public virtual InstitutionProvider? InstitutionProvider { get; set; }
}
}

