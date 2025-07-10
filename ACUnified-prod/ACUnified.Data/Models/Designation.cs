using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ACUnified.Data.Models
{
    public class Designation : BaseClass
    {
        //public long Id { get; set; }
         [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
         [Column(TypeName = "varchar(50)")]
        public string Description { get; set; }
          

    }
}

