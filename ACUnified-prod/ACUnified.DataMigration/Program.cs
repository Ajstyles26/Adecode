// See https://aka.ms/new-console-template for more information
using ACUnified.Data.Models;
using CsvHelper;
using CsvHelper.Configuration;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ACUnified.Data.Enum;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using ACUnified.Business.Services.IServices;
using Microsoft.AspNet.Identity;
namespace ACUnified.DataMigration;
public class Program
{



    [STAThread]
    public static void Main()
    {

        Console.Write("=================Starting Data Seeding Process Press Any Key To Start=====================");
        //Console.ReadKey();
        var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();




        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).EnableSensitiveDataLogging().LogTo(Console.WriteLine, LogLevel.Information);



        using (var context = new ApplicationDbContext(optionsBuilder.Options))
        {
            // ... (data seeding logic)
            // Read and clean CSV data
            //Load Country
            var countries = ReadAndCleanCsvData<Country>("SeedData/CountryList.csv");
            var states = ReadAndCleanCsvData<State>("SeedData/StateList.csv");
            var LGAs = ReadAndCleanCsvData<LGA>("SeedData/LGAList.csv");
            var SSCESubjects = ReadAndCleanCsvData<SubjectSetup>("SeedData/SubjectSetupList.csv");
            var UTMESubjects = ReadAndCleanCsvData<UTMESubject>("SeedData/UTMESubjectList.csv");
            var Degree = ReadAndCleanCsvData<Degree>("SeedData/DegreeList.csv");
            var StudentLevel = ReadAndCleanCsvData<Level>("SeedData/StudentLevelList.csv");
            var Faculties = ReadAndCleanCsvData<Faculty>("SeedData/FacultyList.csv");
            var Departments = ReadAndCleanCsvData<Department>("SeedData/DepartmentList.csv");
            var ProgramSetup = ReadAndCleanCsvData<ProgramSetup>("SeedData/ProgramSetup.csv");
            var ApplicantPayCategory = ReadAndCleanCsvData<ApplicantPayCategory>("SeedData/ApplicantPayCategory.csv");
            var ApplicantPayDetails = ReadAndCleanCsvData<ApplicantPayDetails>("SeedData/ApplicantPayDetails.csv");
            var Session = ReadAndCleanCsvData<Session>("SeedData/Session.csv");

            var Semester = ReadAndCleanCsvData<Semester>("SeedData/Semester.csv");

            var CourseList = ReadAndCleanCsvData<Course>("SeedData/Course.csv");
            var StudentList = ReadAndCleanCsvData<Student>("SeedData/Student.csv");
            var StudentEnrolmentList = ReadAndCleanCsvData<StudentEnrolment>("SeedData/StudentEnrolment.csv");

            var CourseRegistrationList = ReadAndCleanCsvData<CourseRegistration>("SeedData/CourseRegistration.csv");

            var StudentResultList = ReadAndCleanCsvData<StudentResult>("SeedData/StudentResult.csv");

            var PaymentHistoryList = ReadAndCleanCsvData<Payment>("SeedData/PaymentHistory.csv");

            // Disable tracking and identity insert for better performance
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Console.WriteLine("Finalizing Seeding Process...");
            // Configure bulk insert options
            // Create PasswordHasher instance
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            List<ApplicationUser> users = new List<ApplicationUser>();

            var entitiesToAdd = new List<IEnumerable<object>>
            {
                countries,
                states,
                LGAs,
                SSCESubjects,
                UTMESubjects,
                StudentLevel,
                Degree,
                Faculties,
                Departments,
                ProgramSetup,
                Session,
                Semester,
                ApplicantPayCategory,
                ApplicantPayDetails,
                CourseList,
                StudentList,
                StudentEnrolmentList,
                CourseRegistrationList,
                StudentResultList,
                PaymentHistoryList
            };

            //last for each to update the userid for user to 

            //context.Database.ExecuteSqlRawAsync("SET GLOBAL local_infile = true");

            context.Database.ExecuteSqlRaw("DELETE FROM ApplicantPayDetails");
            context.Database.ExecuteSqlRaw("DELETE FROM ApplicantPayCategory");
            context.Database.ExecuteSqlRaw("DELETE FROM CourseRegistration");
            context.Database.ExecuteSqlRaw("DELETE FROM StudentResult");


            context.Database.ExecuteSqlRaw("DELETE FROM Country");
            context.Database.ExecuteSqlRaw("DELETE FROM State");
            context.Database.ExecuteSqlRaw("DELETE FROM LGA");

            context.Database.ExecuteSqlRaw("DELETE FROM Semester");

            context.Database.ExecuteSqlRaw("DELETE FROM Session");
            context.Database.ExecuteSqlRaw("DELETE FROM ProgramSetup");
            context.Database.ExecuteSqlRaw("DELETE FROM SubjectSetup");
            context.Database.ExecuteSqlRaw("DELETE FROM UTMESubject");

            context.Database.ExecuteSqlRaw("DELETE  FROM Student");
            context.Database.ExecuteSqlRaw("DELETE  FROM StudentEnrolment");
            context.Database.ExecuteSqlRaw("DELETE  FROM Degree");
            context.Database.ExecuteSqlRaw("DELETE  FROM Level");
            context.Database.ExecuteSqlRaw("DELETE  FROM Course");
            context.Database.ExecuteSqlRaw("DELETE  FROM Department");
            context.Database.ExecuteSqlRaw("DELETE  FROM Faculty");
            context.Database.ExecuteSqlRaw("DELETE  FROM AspNetUsers");
            context.Database.ExecuteSqlRaw("DELETE  FROM Payment");

            context.Database.ExecuteSqlRaw("INSERT  INTO AspNetUsers (Id, CurrentUserType, UWPId, CurrentApplicationStage, ACUNo, MatricNo, Firstname, Surname, Othername, `Session`, ProfilePicture, InstitutionProviderId, AuthenticationStatus, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) VALUES('d0b45118-980f-4109-8a7b-434178cba541', 0, 17137682974008712, 0, '', '', 'ICT', 'ICT', 'ICT', 'N/A', '/upload/blank-person.png', NULL, 0, 'so.alade@acu.edu.ng', 'SO.ALADE@ACU.EDU.NG', 'so.alade@acu.edu.ng', 'SO.ALADE@ACU.EDU.NG', 0, 'AQAAAAIAAYagAAAAEE1AxsxenTi7dhS+857uJNxP3dLaxKwdxPm2BYsjoN5sDN6VsXN6l2WHvC7gS8GNPQ==', 'AR7BLW4EG3BW4BTLIP3SF3L3PJKD65UZ', 'adbcf717-8ce7-4064-920f-8b77f88e89c9', '', 0, 0, NULL, 1, 0);");

            Console.WriteLine("Creating New Student");
            foreach (var row in StudentList)
            {
                string password = row.Matric; // Set password to MatricNo for demonstration
                ApplicationUser tempUser = new ApplicationUser { UserName = row.Matric };
                string hashedPassword = passwordHasher.HashPassword(tempUser, password);
                ApplicationUser user;
                if (row.Email != null)
                {
                    user = new ApplicationUser
                    {
                        UserName = row.Matric,
                        MatricNo = row.Matric,
                        Firstname = row.FirstName,
                        Surname = row.LastName,
                        Email = row.Email,
                        NormalizedEmail = row.Email,
                        NormalizedUserName = row.Matric,
                        UWPId = GenerateUniqueUWPId(),
                        CurrentUserType = UserType.Student,
                        PasswordHash = hashedPassword

                        // Map other properties
                    };

                }
                else
                {
                    user = new ApplicationUser
                    {
                        UserName = row.Matric,
                        MatricNo = row.Matric,
                        Firstname = row.FirstName,
                        Surname = row.LastName,
                        NormalizedUserName = row.Matric,
                        UWPId = GenerateUniqueUWPId(),
                        CurrentUserType = UserType.Student,
                        PasswordHash = hashedPassword
                        // Map other properties
                    };

                }

                Console.WriteLine($"Updating New Users {row.Matric}");
                users.Add(user);
            }
            Console.WriteLine("End Creating New Users");
            // Batch insert userdata

            //update the  password for the users
            Console.WriteLine("Update passwords New Users");

            //Parallel.ForEach(users, user =>
            //{
            //    string password = user.MatricNo; // Set password to MatricNo for demonstration

            //    Console.WriteLine($"Updating New Password for {user.MatricNo}");

            //    // Hash the password
            //    string hashedPassword = passwordHasher.HashPassword(user, password);

            //    // Update the password hash for the user
            //    user.PasswordHash = hashedPassword;
            //});

            Console.WriteLine("Completed password update  process for new Users");
            context.Users.AddRange(users);
            context.SaveChanges();
            Console.WriteLine("Complete user password ");
            Console.WriteLine("Creating New Updating Students");
            Console.WriteLine("Start  New Student update");
            foreach (var student in StudentList)
            {

                //get student which the id belongs to
                var currentUser = users.Where(x => x.MatricNo == student.Matric).FirstOrDefault();
                Console.WriteLine($"Updating New Student {student.Matric}");
                // If a corresponding user is found, set the UserId of the student
                if (currentUser != null)
                {
                    student.UserId = currentUser.Id;
                    student.CurrentUser.Id = currentUser.Id;
                }
                else
                {
                    // Handle the case where a corresponding user is not found
                    // For example, log a message or handle the situation based on your requirement
                    Console.WriteLine($"User not found for student with MatricNo: {student.Matric}");
                }
            }
            Console.WriteLine("Finish  New Student update");


            foreach (var entityList in entitiesToAdd)
            {
                try
                {
                    Console.WriteLine("Bulk Insert Process");
                    context.BulkInsert(entityList);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.StackTrace);
                }

            }
            context.SaveChanges();

        }
        Console.WriteLine("Completed Data Seeding Process...");
        Console.ReadKey();
    }
    static List<T> ReadAndCleanCsvData<T>(string csvFilePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {

            MissingFieldFound = null,
            HeaderValidated = null,
            BadDataFound = (context) =>
            {

                Console.WriteLine($"context: {context}");
            },
        };
        var records = new List<T>();
        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, config))
        {
            records = csv.GetRecords<T>().ToList();
            // Implement any necessary data cleaning or transformation logic here
        }
        return records;
    }
    static void DisableForeignKeyChecks(MySqlConnection connection, string tableName)
    {
        string sql = $"SET FOREIGN_KEY_CHECKS = 0;";
        ExecuteNonQuery(connection, sql);
    }
    static void ExecuteNonQuery(MySqlConnection connection, string sql)
    {
        using (var command = new MySqlCommand(sql, connection))
        {
            command.ExecuteNonQuery();
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

