using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models
{
    public class Subject : BaseClass
    {
        //public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Grade { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Description { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser CurrentUser { get; set; }
    }
}

