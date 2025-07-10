namespace ACUnified.Data.DTOs;
public class ApplicantCategoryDto
{
    public string ProgramName { get; set; }
    public decimal Amount { get; set; }
    //institution provider
    public virtual long? InstitutionProviderId { get; set; }

}