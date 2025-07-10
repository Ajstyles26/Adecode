using ACUnified.Business.Repository;
using ACUnified.Business.Repository.IRepository;
using ACUnified.Data.Models;
using MudBlazor.Services;
using ACUnified.Portal.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ACUnified.Portal.Hubs;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.MySql;
using ACUnified.Business.Services.IServices;
using ACUnified.Business.Services;
using Microsoft.AspNetCore.HttpOverrides;
using ACUnified.Business.IServices;
using Blazored.Toast;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");
var Configuration = builder.Configuration;

// Replace AddDbContext with AddDbContextFactory
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 31)), 
        b => b.MigrationsAssembly("ACUnified.Portal"))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, LogLevel.Information));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add core services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddSignalR();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddBlazoredToast();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
// Add scoped services
builder.Services.AddScoped<IPasswordPolicyService, PasswordPolicyService>();
builder.Services.AddScoped<IRolesManagement, RolesManagement>();
builder.Services.AddTransient<IACUEmailSender, ACUEmailService>();
builder.Services.AddScoped<IHTMLTemplateFileService, HTMLTemplateFileService>();
builder.Services.AddScoped<IHTML2PDFService, HTML2PDFService>();
builder.Services.AddScoped<IApplicantFeeGenerationService, ApplicantFeeGenerationService>();
builder.Services.AddScoped<ICsvExportService, CsvExportService>();
builder.Services.AddScoped<IApplicantDebtorListExportService, ApplicantDebtorListExportService>();
builder.Services.AddScoped<IHostelDebtorListExportService, HostelDebtorListExportService>();
builder.Services.AddScoped<ICsvExportServices, CsvExportServices>();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add repositories
builder.Services.AddScoped<IBioDataRepository, BioDataRepository>();
builder.Services.AddScoped<IHostelPaymentMigrationService, HostelPaymentMigrationService>();
builder.Services.AddScoped<IApplicantDebtorListRepository, ApplicantDebtorListRepository>();
builder.Services.AddScoped<IHostelDebtorListRepository, HostelDebtorListRepository>();
builder.Services.AddScoped<ICompanyInfoRepository, CompanyInfoRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<ILevelRepository, LevelRepository>();
builder.Services.AddScoped<IDegreeRepository, DegreeRepository>();
builder.Services.AddScoped<ILGARepository, LGARepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRegistrationService, CourseRegistrationService>();
builder.Services.AddScoped<INextOfKinRepository, NextOfKinRepository>();
builder.Services.AddScoped<IProgramChoiceRepository, ProgramChoiceRepository>();
builder.Services.AddScoped<IAcademicSessionRepository, AcademicSessionRepository>();
builder.Services.AddScoped<IAcademicSemesterRepository, AcademicSemesterRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentResultRepository, StudentResultRepository>();
builder.Services.AddScoped<IStudentEnrolmentRepository, StudentEnrolmentRepository>();
builder.Services.AddScoped<ISubjectSetupRepository, SubjectSetupRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IPDFRepository, PDFRepository>();
builder.Services.AddScoped<IHealthAppointmentRepository, HealthAppointmentRepository>();
builder.Services.AddScoped<IProgramSetupRepository, ProgramSetupRepository>();
builder.Services.AddScoped<IPayCategoryRepository, PayCategoryRepository>();
builder.Services.AddScoped<IPayDetailsRepository, PayDetailsRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ICourseRegistrationRepository, CourseRegistrationRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<IOtherDetailsRepository, OtherDetailsRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IAcademicQualificationRepository, AcademicQualificationRepository>();
builder.Services.AddScoped<IReferenceRepository, ReferenceRepository>();
builder.Services.AddScoped<IEducationalRecordRepository, EducationalRecordRepository>();
builder.Services.AddScoped<IApplicantPayCategoryRepository, ApplicantPayCategoryRepository>();
builder.Services.AddScoped<IPaymentLogRepository, PaymentLogRepository>();
builder.Services.AddScoped<IApplicantPaymentLogRepository, ApplicantPaymentLogRepository>();
builder.Services.AddScoped<IApplicantPayDetailsRepository, ApplicantPayDetailsRepository>();
builder.Services.AddScoped<IApplicantPaymentRepository, ApplicantPaymentRepository>();
builder.Services.AddScoped<ITransferFormRepository, TransferFormRepository>();
builder.Services.AddScoped<IApplicantPayUploadCategoryRepository, ApplicantPayUploadCategoryRepository>();
builder.Services.AddScoped<IApplicantPayUploadCategoryBatchRepository, ApplicantPayUploadCategoryBatchRepository>();
builder.Services.AddScoped<IApplicationDocumentRepository, ApplicationDocumentRepository>();
builder.Services.AddScoped<IApplicationFormRepository, ApplicationFormRepository>();
builder.Services.AddScoped<IApplicationProgramRepository, ApplicationProgramRepository>();
builder.Services.AddScoped<IDebtorsListRepository, DebtorsListRepository>();
builder.Services.AddScoped<IHostelBuildingRepository, HostelBuildingRepository>();
builder.Services.AddScoped<IHostelAllocationRepository, HostelAllocationRepository>();
builder.Services.AddScoped<IHostelRoomsRepository, HostelRoomsRepository>();
builder.Services.AddScoped<IPayConcessionRepository, PayConcessionRepository>();
builder.Services.AddScoped<IPayScheduleService, PayScheduleService>();
builder.Services.AddScoped<IPayUploadCategoryRepository, PayUploadCategoryRepository>();
builder.Services.AddScoped<IPayUploadCategoryBatchRepository, PayUploadCategoryBatchRepository>();
builder.Services.AddScoped<IPayUploadConcessionRepository, PayUploadConcessionRepository>();
builder.Services.AddScoped<IPayUploadDetailsRepository, PayUploadDetailsRepository>();
builder.Services.AddScoped<IUTMESubjectRepository, UTMESubjectRepository>();
builder.Services.AddScoped<IRemitaNotificationRepository, RemitaNotificationRepository>();
builder.Services.AddScoped<IApplicantPaymentMigrationService, ApplicantPaymentMigrationService>();
builder.Services.AddScoped<IMatriculationService, MatriculationService>();
// builder.Services.AddScoped<AdmissionService>();


// Add HttpClient
builder.Services.AddHttpClient();

// Configure Hangfire
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseStorage(new MySqlStorage(builder.Configuration.GetConnectionString("DefaultConnection"), 
            new MySqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(10),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 25000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "Hangfire",
            }));
});
builder.Services.AddHangfireServer(options => options.WorkerCount = 1);

// Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ACUnified.Portal", Version = "v1" });
});
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<FingerprintHub>("/FingerPrintHub");

// Configure Hangfire dashboard
app.UseHangfireDashboard();

// Configure Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ACUnified.Portal V1");
});

// Configure endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
    {
        IgnoreAntiforgeryToken = true
    });
});

app.Run();