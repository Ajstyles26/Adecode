using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs;

public class ProgramSetupDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long FacultyId { get; set; }
    public virtual Faculty Faculty { get; set; }
    public long DepartmentId { get; set; }
    public virtual Department Department { get; set; }
    //institution provider
    public long? InstitutionProviderId { get; set; }
    public int Quota { get; set; } = 10;
}