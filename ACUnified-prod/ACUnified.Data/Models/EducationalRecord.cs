using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.Models
{
	public class EducationalRecord : BaseClass
    {
        //public long Id { get; set; }
       // public long UserId { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Name { get; set; }
         [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Qualification { get; set; }
         [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Grade { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DocumentPath { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser CurrentUser { get; set; }

        public long? ApplicationFormId { get; set; }
        public ApplicationForm? ApplicationForm { get; set; }
    }
}

