using ACUnified.Data.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models
{
    public class HStudentMasterRecord : BaseClass
    {
        //public long Id { get; set; }
        //Include Biodata Information
        public long? StudentId {get;set;}
        public Student? Student { get;set;}  

        public long? StudentOtherDetailsId {get;set;}
        public StudentOtherDetails? StudentOtherDetails { get;set;}

        public long? StudentAcademicQualificationId { get; set; }
        public StudentAcademicQualification? StudentAcademicQualification { get; set; }

        // public virtual ICollection<OLevelCredentials> OlevelCredentials { get; set; }

        public long? ProgramChoiceId { get; set; }
        public virtual ProgramChoice? ProgramChoice { get; set; }

        public long? StudentNextOfKinId { get; set; }
        public virtual StudentNextOfKin? NextOfKin { get; set; }

        public long? StudentApplicationDocument1Id { get; set; }
        public virtual StudentApplicationDocument? ApplicationDocument1 { get; set; }

        public long? StudentApplicationDocument2Id { get; set; }
        public virtual StudentApplicationDocument? ApplicationDocument2 { get; set; }

        public long? StudentApplicationDocument3ID { get; set; }
        public virtual StudentApplicationDocument? ApplicationDocument3 { get; set; }

        public long? StudentApplicationDocument4Id { get; set; }
        public virtual StudentApplicationDocument? ApplicationDocument4 { get; set; }

        public long? StudentApplicationDocument5Id { get; set; }
        public virtual StudentApplicationDocument? ApplicationDocument5 { get; set; }

        public long? StudentApplicationDocument6Id { get; set; }
        public virtual StudentApplicationDocument? ApplicationDocument6 { get; set; }

        public long? StudentApplicationDocument7Id { get; set; }
        public virtual StudentApplicationDocument? ApplicationDocument7 { get; set; }

        public virtual ICollection<EmploymentRecord> EmploymentRecord { get; set; }
        
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }

        public virtual long? SessionId { get; set; }
        public virtual Session? Session { get; set; }

        public string? FormRefNo { get; set; }
        public string? ApplicationPaymentReference { get; set; }
        public ApplicationStage ApplicantStage { get; set; }
        public string? ApprovedCourse { get; set; }

        public string? DecisionComment { get; set; }

        public DateTime? DecisionDate { get; set; }

        //      programCategories: ProgramCategoryType[]

        // qualifications: string[]
        //dateRanges: {
        //  startDate: string[]
        //  endDate: string[]
        //  }
        //  educationalRecords: EducationalRecord[]
        //  oLevelResults: OLevelResult[]
        //  employmentRecords: EmploymentRecord[]
        //  subjects: SubjectInterface[]
        //  grades: string[]
    }
}

