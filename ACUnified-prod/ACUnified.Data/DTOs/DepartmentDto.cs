using System;
using ACUnified.Data.Models;
namespace ACUnified.Data.DTOs
{
    public class DepartmentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long FacultyId { get; set; }
        public virtual Faculty Faculty { get; set; }
        //institution provider
        public long? InstitutionProviderId { get; set; }
    }
}

