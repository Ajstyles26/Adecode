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
    public class StudentResultUpload : BaseClass
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? ACUNo { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string CourseCode { get; set; }
        public decimal Score { get; set; }

        public Semester Semester { get; set; }
        public long? SemesterId { get; set; }
        public Session Session { get; set; }
        public long? SessionId { get; set; }
        //this is the user who uploaded the result
        [ForeignKey("ApplicationUser")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }

        public ProcessStatus ResultProcessStatus { get; set; }
    }
}
