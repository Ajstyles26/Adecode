using System;
namespace ACUnified.Data.DTOs
{
    public class SessionDto
    {
        public long Id { get; set; }
        public bool isActive { get; set; }
        public bool isApplicantActive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

