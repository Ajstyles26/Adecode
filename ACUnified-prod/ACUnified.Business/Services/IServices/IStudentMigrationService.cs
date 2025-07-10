using ACUnified.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IStudentMigrationService
    {
        Task MigrateStudentToUser();
    }
}
