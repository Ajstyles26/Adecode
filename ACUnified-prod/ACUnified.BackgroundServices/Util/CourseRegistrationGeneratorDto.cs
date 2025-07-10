using ACUnified.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.BackgroundServices.Util
{
    public class CourseRegistrationGeneratorDto
    {

        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string Slogan { get; set; }
        public DateTime Date { get; set; }

        public IEnumerable<StudentDto> Students { get; set; }
        public IEnumerable<CourseDto> Courses { get; set; }


    }
}
