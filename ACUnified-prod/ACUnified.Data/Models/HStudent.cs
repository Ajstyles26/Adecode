using ACUnified.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;
using System.ComponentModel.DataAnnotations;


namespace ACUnified.Data.Models
{
    public class HStudent : BaseClass
    {

        //   public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string OtherName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string MobileNumber { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Address { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Matric { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string RefNo { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string State { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LGA { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Gender { get; set; }
        
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]public DateTime DOB { get; set; }
        public string MaritalStatus { get; set; }
        
        public string Nationality { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }
        public long LevelId { get; set; }
        public virtual Level Level { get; set; }
        public long SessionId { get; set; }
        public virtual Session Session { get; set; }
        public long ProgramSetupId { get; set; }
        public virtual ProgramSetup ProgramSetup { get; set; }
        public long DegreeId { get; set; }
        public virtual Degree Degree { get; set; }
        public int YearOfEntry { get; set; }

        public bool IsGraduate { get; set; }

        // public Enum.Semester Semester { get; set; }

        public string? UserImage { get; set; }
        public string? FingerUrl { get; set; }

        //student entry type
        public EntryMode? StudentEntryMode { get; set; } = EntryMode.UTME;

        public Enum.CaptureStatus CaptureStatus{get;set;}

        public DateTime? DateCaptured { get; set; }

        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }
    }
}

