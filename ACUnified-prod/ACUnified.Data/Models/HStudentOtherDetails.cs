using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Data.Models
{
    public class HStudentOtherDetails : BaseClass
    {
        public long Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string RedAddress { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string PostalAddress { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Denomination { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ParentName { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ContactAddress { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string FatherNo { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string MotherNo { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ParentEmail { get; set; }


        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        public virtual ApplicationUser? CurrentUser { get; set; }

    }
}
