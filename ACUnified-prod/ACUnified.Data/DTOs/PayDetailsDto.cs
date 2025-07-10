using System;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs
{
    public class PayDetailsDto
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public decimal OriginalAmount { get; set; }

        public string Description { get; set; }

        public long? PayCategoryId { get; set; }

        public virtual PayCategory PayCategory { get;set; }

        public bool isForCourseRegistration { get; set; }

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

   
        public long? StudentId { get; set; }
        public virtual Student? Student { get; set; }


        public long? PayConcessionId { get; set; }
        public virtual PayConcession? PayConcession { get; set; }

        public bool IsForCourseRegistration { get; set; }

        public bool PartAllowed { get; set; }
      
        public bool isLateFee { get; set; } = false;

        public long? SessionId { get; set; }
        public virtual Session? Session { get; set; }
        public long? PayUploadCategoryBatchId { get; set; }
        public virtual PayUploadCategoryBatch? PayUploadCategoryBatch { get; set; }

    }
}

