using ACUnified.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Models
{
    public class PayUploadDetailsRepositoryDto 
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public decimal OriginalAmount { get; set; } = 0;
        /// <summary>
        /// original minus concession amount is total amount due for the semester
        /// </summary>
        public decimal ConcessionAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;

        public long? StudentId { get; set; }

        //this is updated whan user picks the pay plan
        public PayPlan ProformaPayPlan { get; set; }

        public virtual Student Student { get; set; }

        public long? PayCategoryId { get; set; }

        public virtual PayCategory PayCategory { get; set; }

        public long? PayConcessionId { get; set; }
        public virtual PayConcession PayConcession { get; set; }
    }
}
