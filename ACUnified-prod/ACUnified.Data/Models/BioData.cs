using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ACUnified.Data.DTOs;
namespace ACUnified.Data.Models

{
	public class BioData:BaseClass
	{
        //public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)] 
        public string LastName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? OtherName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? FormRefNo { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string MobileNumber { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Address { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string Email { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string State { get; set; }   
       
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LGA { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Gender { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public DateTime DOB { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string MaritalStatus { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Nationality { get; set; }
       
        
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }
        
        public long? LevelId { get; set; }
        public virtual Level? Level { get; set; }
        
        public long? SessionId { get; set; }
        public virtual Session Session { get; set; }
        
        public long? DegreeId { get; set; }
        public virtual Degree Degree { get; set; }
        //institution provider
        public virtual long? InstitutionProviderId { get; set; }
        public virtual InstitutionProvider? InstitutionProvider { get; set; }

        public static implicit operator BioData(BioDataDto v)
        {
            throw new NotImplementedException();
        }
    }
}

