using ACUnified.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

namespace ACUnified.Data.Models
{
    public class Student : BaseClass
    {

        //   public long Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? LastName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? FirstName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? OtherName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Address { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Matric { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? RefNo { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? State { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? LGA { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string? Gender { get; set; }
        public DateTime DOB { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string MaritalStatus { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Nationality { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; } 
        public virtual ApplicationUser? CurrentUser { get; set; } 

        public long? DegreeId { get; set; }
        public virtual Degree? Degree { get; set; }
        
        public int? YearOfEntry { get; set; }

        public bool? IsGraduate { get; set; }

        // public Enum.Semester Semester { get; set; }

        public string? UserImage { get; set; }
        public string? FingerUrl { get; set; }

        //student entry type
        public EntryMode? StudentEntryMode { get; set; } = EntryMode.UTME;

        public Enum.CaptureStatus? CaptureStatus{get;set;}

        public DateTime? DateCaptured { get; set; }

        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }
        //process for converting student to user
        public MigrationStatus MigrationStatus { get; set; } = MigrationStatus.Unmigrated;
    }
}

