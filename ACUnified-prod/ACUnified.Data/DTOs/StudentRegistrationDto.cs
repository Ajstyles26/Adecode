using System;
using ACUnified.Data.Models;
using ACUnified.Data.Enum;

namespace ACUnified.Data.DTOs
{
    public class StudentRegistrationDto
    {
        public string MatricNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // e.g., "Pending", "Registered", "Failed"
        public double Progress { get; set; } = 0; // 0 to 100
    }
}