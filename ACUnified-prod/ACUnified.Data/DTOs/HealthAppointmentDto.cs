using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACUnified.Data.DTOs;

public class HealthAppointmentDto
{
    public long Id { get; set; }
    public string Comment { get; set; }
    public string Purpose { get; set; }
    public AppointmentStatus CurrentStatus { get; set; }
    
    public string? UserId { get; set; }

    

}