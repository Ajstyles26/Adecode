using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models
{
    public class StudentResult : BaseClass
    {

        //public long Id { get; set; }
        //public decimal? SemesterGPA { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? MatricNo { get; set; }
        //[ForeignKey("Student")]
        public long? StudentId { get; set; }
        public virtual Student? Student { get; set; }

        public long? CourseId { get; set; }
        public virtual Course? Course { get; set; }

        public long? SessionId { get; set; }
        public virtual Session? Session { get; set; }

        public long? SemesterId { get; set; }
        public virtual Semester? Semester { get; set; }

        public long? LevelId { get; set; }
        public virtual Level? Level { get; set; }

        public decimal? CA { get; set; }

        public decimal? Exam { get; set; }

        public decimal? Total { get; set; }

        public string? Grade { get; set; }
        //public int? Point { get; set; }

        //[ForeignKey("ApplicationUser")]
        //public long? ProcessorId { get; set; }
        //public virtual ApplicationUser? ProcessedBy { get; set; }
        //public Enum.ProcessStatus? ResultStatus { get; set; } = Enum.ProcessStatus.Completed;
        //institution provider
        //public virtual long? InstitutionProviderId { get; set; }
        //public virtual InstitutionProvider? InstitutionProvider { get; set; }

    }
}

