
namespace ACUnified.Data.DTOs
{
    public class DegreeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        public virtual long? InstitutionProviderId { get; set; }
 
    }
}
