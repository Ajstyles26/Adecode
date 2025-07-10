using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models
{
    public class HStudentNextOfKin : BaseClass
    {
        public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string OtherName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Email { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string MobileNumber { get; set; }
        public string? Gender { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Occupation { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Relationship { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Address { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string LGA { get; set; }


        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }

    }
}
