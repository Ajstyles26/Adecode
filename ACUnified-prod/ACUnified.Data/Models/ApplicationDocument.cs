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
    public class ApplicationDocument:BaseClass
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string filename { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string filepath { get; set; }
        public DocumentType ApplicationDocumentType { get; set; }
        [ForeignKey("ApplicationUser")]        
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }
    }
}
