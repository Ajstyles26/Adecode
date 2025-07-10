using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ACUnified.Data.Models
{
    public class Faculty : BaseClass
    {
        //public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Name { get; set; }
         [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Description { get; set; }
         [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Code { get; set; }
         
        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }
    }
}

