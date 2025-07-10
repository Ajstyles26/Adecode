using System;
using System.ComponentModel.DataAnnotations;
using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs
{
    public class TransferFormDto
    {
        public long Id { get; set; }
        public string UserId { get; set; }

       
        public string Surname { get; set; }

        
        public string FirstName { get; set; }

        public string Middlename { get; set; }

       
        public string MatricNo { get; set; }

     
        public string Institution { get; set; }

   
        public string Level { get; set; }

        
        public decimal Cgpa { get; set; }

        public bool OLevelDeficiency { get; set; }
        public bool OLevelSubjectsNotRelevant { get; set; }
        public bool NoJAMBAdmission { get; set; }
        public bool PoorAcademicPerformance { get; set; }
        public bool FinancialReason { get; set; }
        public string FinancialReasonExplanation { get; set; }

        public string Crime { get; set; }
        public string Immorality { get; set; }
        public string Cultism { get; set; }
        public string OtherReasons { get; set; }
    }
}