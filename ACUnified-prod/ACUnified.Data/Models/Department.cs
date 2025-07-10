using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ACUnified.Data.Models
{
    public class Department : BaseClass

    {
        //public long Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(50)")] 
        public string Description { get; set; }
         
        public long FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }
        public int LevelLimit { get; set; } = 400;
    }
}

