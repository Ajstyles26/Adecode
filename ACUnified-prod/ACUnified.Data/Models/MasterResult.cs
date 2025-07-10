using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ACUnified.Data.Models;

public class MasterResult:BaseClass
{
    //public long Id { get; set; }
    public long StudentId { get; set; }
    public virtual Student Student { get; set; }
    public string CourseCode { get; set; }
    public int CourseUnit { get; set; }
    public int CurrentSession { get; set; }
    public int CurrentSemester { get; set; }
    public int Level { get; set; }
    public int CA { get; set; }
    public int Exam { get; set; }
    public int Total { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(10)]
    public string Grade { get; set; }
    public int Point { get; set; }
    [Column(TypeName = "VARCHAR")]
    [StringLength(10)]
    public string Status { get; set; }
    
}