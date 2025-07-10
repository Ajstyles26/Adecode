using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ACUnified.Data.Enum;

namespace ACUnified.Data.Models;

public class Semester : BaseClass
{
    public long Id { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string Name { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(50)]
    public string Description { get; set; }

    public long SessionId { get; set; }
    public virtual Session Session { get; set; }
    public bool isActive { get; set; }
    public bool isLate { get; set; }
    public SemesterType SemesterType { get; set; }
}