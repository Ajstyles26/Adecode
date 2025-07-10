using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ACUnified.Data.Models
{
    public class State : BaseClass
    {
       // public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Name { get; set; }
    
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Description { get; set; }
       
        public long CountryId  { get; set; }
        public virtual Country Country { get; set; }
    }
}

