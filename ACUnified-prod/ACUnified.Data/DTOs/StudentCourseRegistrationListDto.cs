using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class StudentCourseRegistrationListDto
    {
        public long? StudentId { get; set; }
        public virtual Student Student { get; set; }
        public long? SemesterId { get; set; }
        public virtual Semester Semester { get; set; }
        public long? SessionId { get; set; }
        public virtual Session Session { get; set; }
        public String? Name { get; set; }
    }
}
