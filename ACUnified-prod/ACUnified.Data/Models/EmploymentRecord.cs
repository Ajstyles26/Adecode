using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models
{
	public class EmploymentRecord : BaseClass
    {
        //public long Id { get; set; }
        
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string EmployerName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Designation { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser CurrentUser { get; set; }
        public long ApplicationFormId { get; set; }
        public virtual ApplicationForm ApplicationForm { get; set; }

    }
}

