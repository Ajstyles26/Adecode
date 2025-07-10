using ACUnified.Data.Enum;

namespace ACUnified.Data.DTOs;

public class SemesterDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long SessionId { get; set; }
    public bool isActive { get; set; }
    public bool isLate { get; set; }
    public SemesterType SemesterType { get; set; }
    
}