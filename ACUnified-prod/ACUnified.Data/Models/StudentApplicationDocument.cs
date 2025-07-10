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
    public class StudentApplicationDocument:BaseClass
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string filename { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string filepath { get; set; }
        public DocumentType ApplicationDocumentType { get; set; }
        [ForeignKey("ApplicationUser")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }

        public long? StudentId { get; set; }
        public Student Student { get; set; }
    }
}
