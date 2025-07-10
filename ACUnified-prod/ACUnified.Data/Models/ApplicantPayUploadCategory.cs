using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Models
{
    public class ApplicantPayUploadCategory:BaseClass
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(80)]
        public string Name { get; set; }
        public decimal TotalPayDue { get; set; }
        public decimal PayInstalment1 { get; set; }
        public decimal PayInstalment2 { get; set; }
        public decimal PayInstalment3 { get; set; }
        //second and third payment fee
        public decimal PayInstalment4 { get; set; }
        public Boolean IsGlobal { get; set; }
        public long? StudentLevelId { get; set; }
        public virtual Level? StudentLevel { get; set; }

        public long? SessionId { get; set; }
        public virtual Session? Session { get; set; }

        public virtual Semester? Semester { get; set; }
        public long? SemesterId { get; set; }

        public long? DegreeId { get; set; }
        public virtual Degree? Degree { get; set; }

        public long? ProgramSetupId { get; set; }
        public virtual ProgramSetup? ProgramSetup { get; set; }

        public long? ApplicantPayUploadCategoryBatchId { get; set; }
        public virtual ApplicantPayUploadCategoryBatch? ApplicantPayUploadCategoryBatch { get; set; }

        public PayCategoryType payCategoryType { get; set; } = PayCategoryType.Tuition;
        public EntryMode entryMode { get; set; } = EntryMode.UTME;

        //institution provider
        public  long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }
    }
}
