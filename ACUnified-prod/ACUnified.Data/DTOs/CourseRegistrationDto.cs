using ACUnified.Data.Enum;
using ACUnified.Data.Models;

namespace ACUnified.Data.DTOs;

public class CourseRegistrationDto
{
    public long Id { get; set; }
    public long? StudentId { get; set; }
    public virtual Student? Student { get; set; }

     public long? ApplicationFormId { get; set; }
        public ApplicationForm? ApplicationForm { get; set; }
    public long CourseId { get; set; }
    public virtual Course Course { get; set; }
    public virtual Semester Semester { get; set; }
    public long? SemesterId { get; set; }
    public long SessionId { get; set; } 
    public virtual Session Session { get; set; }
    public long? StudentEnrolmentId { get; set; }
    public virtual StudentEnrolment StudentEnrolment { get; set; }
      public int TotalCourses { get; set; }
        public int TotalUnits { get; set; }

    public long ProgramSetupId { get; set; }
    public virtual ProgramSetup ProgramSetup { get; set; }
    //institution provider
    public long? InstitutionProviderId { get; set; }
}