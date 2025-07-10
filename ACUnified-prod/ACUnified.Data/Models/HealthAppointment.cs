using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ACUnified.Data.Enum;

namespace ACUnified.Data.Models;

public class HealthAppointment:BaseClass
{
    //public long Id { get; set; }
    [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
    public string Comment { get; set; }
    [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
    public string Purpose { get; set; }
    
    public AppointmentStatus CurrentStatus { get; set; }

    [ForeignKey("ApplicationUser")]
    public string? UserId { get; set; }
    public virtual ApplicationUser? CurrentUser { get; set; }

}