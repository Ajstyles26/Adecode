using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace ACUnified.Data.Models
{
    public class CompanyInfo:BaseClass
    {
        //public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)] 
        public string Name { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)] 
        public string Description { get; set; }
    }
}

