using ACUnified.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Models
{
    public class PayUploadDetails:BaseClass
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(70)]
        public string Name { get; set; }

        public decimal OriginalAmount { get; set; } = 0;
        /// <summary>
        /// original minus concession amount is total amount due for the semester
        /// </summary>
        public decimal ConcessionAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
        public decimal PayInstalment1 { get; set; } = 0;
        public decimal PayInstalment2 { get; set; } = 0;
        public decimal PayInstalment3 { get; set; } = 0;
        //second and third payment fee
        public decimal PayInstalment4 { get; set; } = 0;

        //this is updated whan user picks the pay plan
        public PayPlan? ProformaPayPlan { get; set; }
        public long? StudentId { get; set; }
        public virtual Student? Student { get; set; }

        public long? PayUploadCategoryId { get; set; }

        public virtual PayUploadCategory? PayUploadCategory { get; set; }

        public long? PayUploadCategoryBatchId { get; set; }

        public virtual PayUploadCategoryBatch? PayUploadCategoryBatch { get; set; }

        public long? PayUploadConcessionId { get; set; }
        public virtual PayUploadConcession? PayUploadConcession { get; set; }


        public bool IsForCourseRegistration { get; set; }

        public bool PartAllowed { get; set; } = true;

        public bool isLateFee { get; set; } = false;
        public long? SessionId { get; set; }
        public virtual Session? Session { get; set; }
    }
}
