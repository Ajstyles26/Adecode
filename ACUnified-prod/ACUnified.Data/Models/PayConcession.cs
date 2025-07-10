using ACUnified.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.Models
{
    public class PayConcession:BaseClass
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Name { get; set; }
        public ConcessionType PayConcessionType { get; set; }
        public decimal? Amount { get; set; }
        public long? DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public long? FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }

        public long? StudentId { get; set; }

        public virtual Student Student { get; set; }

        public long? PayUploadCategoryBatchId { get; set; }

        public virtual PayUploadCategoryBatch? PayUploadCategoryBatch { get; set; }
    }
}
