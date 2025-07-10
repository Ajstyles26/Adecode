using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models
{
    public class Reference : BaseClass
    {

        // public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? FullName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Designation { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string? Residential { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? MobileNumber { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Email { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? FullName2 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Designation2 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string? Residential2 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? MobileNumber2 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Email2 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? FullName3 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Designation3 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string? Residential3 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? MobileNumber3 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string? Email3 { get; set; }


        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }

    }
}

