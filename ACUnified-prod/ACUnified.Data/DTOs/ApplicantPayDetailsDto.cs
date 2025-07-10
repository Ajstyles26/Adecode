using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class ApplicantPayDetailsDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public long? ApplicantPayCategoryId { get; set; }

        public virtual ApplicantPayCategory ApplicantPayCategory { get; set; }

        public long? SessionId { get; set; }
        public Session? Session { get; set; }
        public SemesterType Semester { get; set; } = SemesterType.First;
        public bool isForCourseRegistration { get; set; }

        public ApplicantFeeType? ApplicantFeeType { get; set; } = Enum.ApplicantFeeType.ApplicationForm;
        public decimal ConcessionAmount { get; set; } = 0;

        public decimal PayInstalment1 { get; set; } = 0;
        public decimal PayInstalment2 { get; set; } = 0;
        public decimal PayInstalment3 { get; set; } = 0;
        //second and third payment fee
        public decimal PayInstalment4 { get; set; } = 0;
        public bool partAllowed { get; set; } = false;
        
         public long? ProgramSetupId { get; set; }
        public virtual ProgramSetup? ProgramSetup { get; set; }
        
    }
}
