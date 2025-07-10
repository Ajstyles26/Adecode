using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs;

public class MasterResultDto
{
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
    public string Grade { get; set; }
    public int Point { get; set; }
    public string Status { get; set; }
}