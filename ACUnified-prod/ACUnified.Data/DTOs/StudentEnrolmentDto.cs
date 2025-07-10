using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class StudentEnrolmentDto
    {
        public long Id { get; set; }
        public long ProgramSetupId { get; set; }
        public virtual ProgramSetup ProgramSetup { get; set; }
        public long DegreeId { get; set; }
        public virtual Degree Degree { get; set; }
        public long StudentId { get; set; }
        public virtual Student Student { get; set; }
        public long LevelId { get; set; }
        public virtual Level Level { get; set; }

    }
}
