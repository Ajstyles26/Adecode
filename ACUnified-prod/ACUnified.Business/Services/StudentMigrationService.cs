using ACUnified.Business.Services.IServices;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using ACUnified.Portal.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services
{
    public class StudentMigrationService : IStudentMigrationService
    {
        IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        IRolesManagement _rolesManagement;
        public StudentMigrationService(IStudentRepository studentRepository, UserManager<ApplicationUser> userManager, IRolesManagement rolesManagement)
        {
            _studentRepository = studentRepository;
            _userManager = userManager;
            _rolesManagement = rolesManagement;

        }
        public async Task MigrateStudentToUser()
        {
            //get list of users not migrated
            var unMigratedList=await _studentRepository.GetAllUnMigratedStudent();
            //start the migration
            if (unMigratedList != null)
            {
                foreach (var item in unMigratedList)
                {
                    var firstname = item.FirstName;
                    var surname = item.LastName;
                    var username = item.Matric;
                    var email = item.Email??"";
                    var password = item.Matric;
                    var uwpid = GenerateUniqueUWPId();
                    
                    // Check if the user already exists
                    var existingUser = await _userManager.FindByNameAsync(username);
                    if (existingUser!=null)
                    {
                        var newUser = new ApplicationUser
                        { 
                            Firstname = firstname,
                            Surname = surname,
                            UserName = username,
                            Email = email,
                            UWPId = uwpid,
                            CurrentUserType=UserType.Student,
                            
                            // Add other properties as needed
                        };
                        
                        // Create the user using UserManager
                        var result = await _userManager.CreateAsync(newUser, password);

                        if (result.Succeeded)
                        {
                            // Optionally, add the user to roles or perform other tasks
                            await _rolesManagement.GenerateRolesFromPagesAsync();
                            await _rolesManagement.AddToRoles(newUser);

                            item.MigrationStatus = MigrationStatus.Migrated;
                            //update the record in the database
                            await _studentRepository.UpdateStudent(item);

                        }
                        else
                        {
                            // Handle errors if user creation fails
                        }
                    }
                }
            }
        }

        public static long GenerateUniqueUWPId()
        {
            // Use a combination of timestamp and a random number to generate a unique value
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            int randomSuffix = new Random().Next(1000, 9999); // Adjust the range as needed

            return long.Parse($"{timestamp}{randomSuffix}");
        }
    }
}
