using System;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs
{
    public class CourseDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public int CourseUnit { get; set; }
        public SemesterType Semester { get; set; }
        public string Description { get; set; }
        public CourseCategory courseCategory { get; set; }
        public long FacultyId { get; set; }
        public virtual Faculty Faculty{get;set;}
        public long DegreeId { get; set; }
        public virtual Degree Degree { get; set; }
        public long DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public long ProgramSetupId { get; set; }
        public virtual ProgramSetup ProgramSetup{ get; set; }
        public long LevelId { get; set; }
        public virtual Level Level{ get; set; }
        public string? Status { get; set; }
        public string? Prerequisite { get; set; }
        //institution provider
        public long? InstitutionProviderId { get; set; }
    }
}

