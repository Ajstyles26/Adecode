using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.DTOs
{
    public class ApplicationFormDto
    {
        // Existing properties
        public long Id { get; set; }
        //Include Biodata Information
        public long? BioDataId { get; set; }
        public BioData BioData { get; set; }

        public long? OtherDetailsId { get; set; }
        public OtherDetails OtherDetails { get; set; }

        public long? AcademicQualificationId { get; set; }
        public AcademicQualification AcademicQualification { get; set; }


        public long? ReferenceId { get; set; }
        public Reference References { get; set; }

        public bool Iagree { get; set; }

        public long? ProgramChoiceId { get; set; }
        public virtual ProgramChoice ProgramChoice { get; set; }

        public long? ProgramSetupId { get; set; }
        public virtual ProgramSetup? ProgramSetup { get; set; }

        public long? DegreeId { get; set; }
        public virtual Degree Degree { get; set; }

        public long? LevelId { get; set; }
        public virtual Level? Level { get; set; }

        public virtual ICollection<EmploymentRecord> EmploymentRecord { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }

        public virtual long? SessionId { get; set; }
        public virtual Session? Session { get; set; } 
        public long? NextOfKinId { get; set; }
        public virtual NextOfKin? NextOfKin { get; set; }

        public long? ApplicationDocument1ID { get; set; }
        public virtual ApplicationDocument? ApplicationDocument1 { get; set; }

        public long? ApplicationDocument2ID { get; set; }
        public virtual ApplicationDocument? ApplicationDocument2 { get; set; }

        public long? ApplicationDocument3ID { get; set; }
        public virtual ApplicationDocument? ApplicationDocument3 { get; set; }

        public long? ApplicationDocument4ID { get; set; }
        public virtual ApplicationDocument? ApplicationDocument4 { get; set; }

        public long? ApplicationDocument5ID { get; set; }
        public virtual ApplicationDocument? ApplicationDocument5 { get; set; }

        public long? ApplicationDocument6ID { get; set; }
        public virtual ApplicationDocument? ApplicationDocument6 { get; set; }

        public long? ApplicationDocument7ID { get; set; }
        public virtual ApplicationDocument? ApplicationDocument7 { get; set; }

        public string? FormRefNo { get; set; }
        public string ApplicationPaymentReference { get; set; }
        public ApplicationStage ApplicantStage { get; set; }
        public string? ApprovedCourse { get; set; }

        public string? DecisionComment { get; set; }

        public string? Decision { get; set; }
        public DateTime? DecisionDate { get; set; }
        public BTHBAConversionCategory BTHConversionCategory { get; set; } = BTHBAConversionCategory.None;

        public string? BTHCenter { get; set; }
        public string? FinalisedCourse { get; set; }

        public string? FinalisedDecision { get; set; }

        public string? FinalisedComment { get; set; }

        public DateTime? FinalDecisionDate { get; set; }

        public string? FinalizedUserId { get; set; }

        public virtual ApplicationUser? FinalizedUser { get; set; }
        public string? DecisionMakerUserId { get; set; }

        public virtual ApplicationUser? DecisionMakerUser { get; set; }

        public long? TransferFormId { get; set;}
        public TransferForm TransferForm { get; set;}

        // New properties for matriculation
        public string? MatriculationNumber { get; set; }
        public DateTime? MatriculationDate { get; set; }
    }
}