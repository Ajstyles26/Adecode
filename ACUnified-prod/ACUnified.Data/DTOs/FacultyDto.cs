using System;
namespace ACUnified.Data.DTOs
{
    public class FacultyDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //institution provider
        public long? InstitutionProviderId { get; set; }
    }
}

