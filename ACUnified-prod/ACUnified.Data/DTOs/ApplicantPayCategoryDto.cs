using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Data.DTOs
{
    public class ApplicantPayCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }


        public decimal TotalPayDue { get; set; }

        public bool IsInstallmental { get; set; }

        public long? StudentLevelId { get; set; }

        public virtual Level StudentLevel { get; set; }

        public long? DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public long? SessionId { get; set; }
        public virtual Session Session { get; set; }

        public virtual Semester Semester { get; set; }
        public long? SemesterId { get; set; }

        public long? DegreeId { get; set; }
        public Degree Degree { get; set; }
        //institution provider
        public long? InstitutionProviderId { get; set; }
    }
}
