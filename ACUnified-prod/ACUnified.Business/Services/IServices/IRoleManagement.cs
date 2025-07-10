using ACUnified.Data.Models;
using ACUnified.Portal.MenuHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IRolesManagement
    {
        Task GenerateRolesFromPagesAsync();
        Task AddToRoles(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> RolebaseMenuLoad(ApplicationUser _ApplicationUser);
    }
}
