using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.BioVerification.Data
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ACUDbContext:DbContext
    {
        public ACUDbContext():base("bioverifyConnection")
        {
            
        }
        public DbSet<Student> Student { get; set; }
    }
}
