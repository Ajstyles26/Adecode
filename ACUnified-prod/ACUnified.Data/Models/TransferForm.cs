using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models
{
    public class TransferForm : BaseClass
    {
      
        //public int Id { get; set; }

       
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string Middlename { get; set; }

        [Required]
        [StringLength(50)]
        public string MatricNo { get; set; }

        [Required]
        [StringLength(200)]
        public string Institution { get; set; }

        [Required]
        [StringLength(50)]
        public string Level { get; set; }

        [Required]
        [Column(TypeName = "decimal(3, 2)")]
        public decimal Cgpa { get; set; }

        public bool OLevelDeficiency { get; set; }
        public bool OLevelSubjectsNotRelevant { get; set; }
        public bool NoJAMBAdmission { get; set; }
        public bool PoorAcademicPerformance { get; set; }
        public bool FinancialReason { get; set; }

        [StringLength(500)]
        public string FinancialReasonExplanation { get; set; }

        [StringLength(500)]
        public string Crime { get; set; }

        [StringLength(500)]
        public string Immorality { get; set; }

        [StringLength(500)]
        public string Cultism { get; set; }

        [StringLength(1000)]
        public string OtherReasons { get; set; }

       
        

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}