using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class ApplicationDocumentDto
    {
        public long Id { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public DocumentType ApplicationDocumentType { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }
    }
}
