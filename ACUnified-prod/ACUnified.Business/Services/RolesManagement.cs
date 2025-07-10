using System.Reflection;
using System.Text.RegularExpressions;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.Models;
using ACUnified.Portal.MenuHelper;
using Microsoft.AspNetCore.Identity;

namespace ACUnified.Portal.Data;

public class RolesManagement:IRolesManagement
	{
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesManagement(RoleManager<IdentityRole> roleManager,
          UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        //Create Roles on the fly for every one 
        public async Task GenerateRolesFromPagesAsync()
        {
            Type t = typeof(MainMenu);
            foreach (Type item in t.GetNestedTypes())
            {
                foreach (var itm in item.GetFields())
                {
                    if (itm.Name.Contains("RoleName"))
                    {
                        string roleName = (string)itm.GetValue(item);
                        if (!await _roleManager.RoleExistsAsync(roleName))
                            await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
        //this is for new use to add roles for new user
        public async Task  AddToRoles(ApplicationUser _ApplicationUser)
        {
            var roles = _roleManager.Roles;
            string[] currentRoles;
            List<string> listRoles = new List<string>();
            if (_ApplicationUser != null)
            {
                    currentRoles = RolePage.GetByAppUserPage(_ApplicationUser.CurrentUserType);
                    var selectedRoles = roles.Where(role => currentRoles.ToList().Contains(role.Name)).ToArray();
                    foreach (var item in selectedRoles)
                    {
                        listRoles.Add(item.Name);
                    }
             }
              await _userManager.AddToRolesAsync(_ApplicationUser, listRoles);
        }
        //this is to load pages based on the user
        public async Task<MainMenuViewModel> RolebaseMenuLoad(ApplicationUser _ApplicationUser)
        {
            MainMenuViewModel _MainMenuViewModel = new MainMenuViewModel();
            PropertyInfo[] _PropertyInfo = typeof(MainMenuViewModel).GetProperties();
            try
            {
                var _Roles =  _roleManager.Roles.ToList();
                foreach (var role in _Roles)
                {
                    var _PropertyName = _PropertyInfo.Where(x => x.Name == Regex.Replace(role.Name, @"\s+", "")).SingleOrDefault();
                    var _IsInRoleAsync = await _userManager.IsInRoleAsync(_ApplicationUser, role.Name);
                    Console.WriteLine(_PropertyName);
                    if (_PropertyName != null) // Check if the property exists
                    {
                        if (_IsInRoleAsync)
                            _PropertyName.SetValue(_MainMenuViewModel, true);
                        else
                            _PropertyName.SetValue(_MainMenuViewModel, false);
                    }
                    else
                    {
                        // Handle the scenario where no matching property is found.
                        // For example, you can log a warning or ignore it.
                        Console.WriteLine($"No property found for role: {role.Name}");
                    }
                }
                return _MainMenuViewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }

   