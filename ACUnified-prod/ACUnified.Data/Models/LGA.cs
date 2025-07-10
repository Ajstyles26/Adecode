using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models
{
    public class LGA : BaseClass
    {
        //public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Name { get; set; }
          [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Description { get; set; }
          
        [ForeignKey("State")]
         public long StateID { get; set; }
        public virtual State State { get; set; }
        
    }
}

