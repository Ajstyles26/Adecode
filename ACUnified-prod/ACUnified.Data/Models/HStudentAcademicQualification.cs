using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models
{
    public class HStudentAcademicQualification : BaseClass
    {

        public long Id { get; set; }



        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ExamType { get; set; }


        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ExamNumber { get; set; }


        public int UTMEScore { get; set; }


        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Subject1 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Grade1 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Subject2 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Grade2 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Subject3 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Grade3 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Subject4 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Grade4 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Subject5 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Grade5 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string UTMESubject1 { get; set; }

        public int UTMESubscore1 { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string UTMESubject2 { get; set; }


        [Column(TypeName = "VARCHAR")]
        [StringLength(50)] public int UTMESubscore2 { get; set; }
        public string UTMESubject3 { get; set; }

        public int UTMESubscore3 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]

        public string UTMESubject4 { get; set; }
        public int UTMESubscore4 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string UTMERegNo { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Modeofentry { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Choice1 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Choice2 { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
    }
}