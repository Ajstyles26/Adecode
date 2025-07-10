// See https://aka.ms/new-console-template for more information
using ACUnified.Business.Repository;
using ACUnified.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;
using AutoMapper;
using ACUnified.Shared.Models;
using ACUnified.Business.Repository.IRepository;
using ACUnified.Portal.Utils;
using System.IO;
using ACUnified.BackgroundServices.NewFolder;
using ACUnified.BackgroundServices.Util;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.Write("Welcome to the course form mass generator");

        var mapConfig = new MapperConfiguration(cfg => { cfg.AddProfile<ACUnified.Business.Mapper.MappingProfile>(); });
        //Student retrieval
        var serviceProvider =new ServiceCollection()
            .AddDbContext<ApplicationDbContext>(options=> options.UseMySql("Server=localhost;Database=uwpdatastore5;User Id=root;Password=Nigeria2022%",
            new MySqlServerVersion(new Version(8,0,31))).EnableSensitiveDataLogging())        
            .AddScoped<IFacultyRepository,FacultyRepository>()
            .AddScoped<IAcademicSessionRepository, AcademicSessionRepository>()
            .AddScoped<IDepartmentRepository, DepartmentRepository>()
            .AddScoped<ICourseRepository, CourseRepository>()
            .AddScoped<IProgramSetupRepository, ProgramSetupRepository>()
            .AddScoped<IStudentRepository, StudentRepository>()
            .AddScoped<ICourseRegistrationRepository, CourseRegistrationRepository>()
            .AddSingleton<IMapper>(sp=>new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ACUnified.Business.Mapper.MappingProfile>();

            }))).BuildServiceProvider();
        ///course reg generator
        using (var scope = serviceProvider.CreateScope())
        {
            var studentRepository = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
            var programRepository = scope.ServiceProvider.GetRequiredService<IProgramSetupRepository>();

            var studentListing = await studentRepository.GetAllStudentCourseReg();
            var programsetup = await programRepository.GetAllProgramSetup();
            var bprogramSetup = programsetup.Where(x => x.InstitutionProviderId == 1);
            var mfr=new MyFontResolver();
            foreach (var item in bprogramSetup)
            {
                var demo = studentListing.Where(x => x.ProgramSetupId == item.Id);
                //select only student from fst

                //program setup

                //fetch course

                // Map the entity to DTO using AutoMapper
                var courseRepository = scope.ServiceProvider.GetRequiredService<ICourseRepository>();
                var coursedemo = await courseRepository.GetStudentCourse(item.DepartmentId, 1, ACUnified.Data.Enum.SemesterType.First);

                CourseRegistrationGeneratorDto courseRegGenDTO = new CourseRegistrationGeneratorDto();
                courseRegGenDTO.Students = demo;
                courseRegGenDTO.Courses = coursedemo;
                courseRegGenDTO.SchoolName = "Bells University of Technology";
                courseRegGenDTO.Address = "Km. 8 Idiroko Rd, Benja village, Ota 112104, Ogun State";
                courseRegGenDTO.Slogan = "...only the best is good for bells";
                var currentstream = CourseFormGenerator.GenerateCourseFormPdf(courseRegGenDTO,mfr);
                // Get the directory where the executable is located
                string directoryPath = AppDomain.CurrentDomain.BaseDirectory;

                // Specify the file path using the executable's directory and the desired file name ("document.pdf")
                string filePath = Path.Combine(directoryPath, $"{item.Name}.pdf");

                // Save the MemoryStream content to a PDF file
                SaveMemoryStreamToFile(currentstream, filePath);
                Console.WriteLine("PDF file saved successfully at: " + filePath);
            }

        }

            Console.WriteLine("Food is ready");
            //end course reg generator
            Console.ReadKey();
    }
    static void SaveMemoryStreamToFile(MemoryStream stream, string filePath)
    {
        // Ensure the stream position is at the beginning
        stream.Position = 0;

        // Create a FileStream to write the stream content to the file
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            // Copy the content of the MemoryStream to the FileStream
            stream.CopyTo(fileStream);
        }
    }

}
