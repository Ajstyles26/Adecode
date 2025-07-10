using System;
using ACUnified.Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ACUnified.Data.Models
{
    //DbContext for normal IdentityDbContext for identity
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
      

        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<ApplicationForm>().Where(e => e.State == EntityState.Added))
            {
                var entity = entry.Entity;

                // Check if the Id is 0 (indicating it's a new entity)
                if (entity.Id == 0)
                {
                    // Perform the database insert asynchronously to get the auto-incremented Id
                    await base.SaveChangesAsync(cancellationToken);

                    // Update the reference number using the generated Id
                    entity.FormRefNo = $"ACU{DateTime.Now.Year}" + entity.Id.ToString("D4");
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<ApplicantPayCategory> ApplicantPayCategory { get; set; }
        public DbSet<ApplicantPayUploadCategory> ApplicantPayUploadCategory { get; set; }
        public DbSet<ApplicantPayUploadCategoryBatch> ApplicantPayUploadCategoryBatch { get; set; }
        public DbSet<ApplicantPayDetails> ApplicantPayDetails { get; set; }
        public DbSet<ApplicantPayment> ApplicantPayment { get; set; }
        public DbSet<ApplicantPaymentLog> ApplicantPaymentLog { get; set; }
        public DbSet<ApplicationDocument> ApplicationDocument { get; set; }
        public DbSet<ApplicationForm> ApplicationForm { get; set; }
        public DbSet<AcademicQualification> AcademicQualification { get; set; }
        public DbSet<ApplicationProgram> ApplicationProgram { get; set; }
        public DbSet<ApplicantDebtorList> ApplicantDebtorList { get; set; }
        public DbSet<HostelDebtorList> HostelDebtorList {get; set;}
        public DbSet<BioData> BioData { get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Reference> Reference { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<CourseRegistration> CourseRegistration { get; set; }
        public DbSet<UTMESubject> UTMESubject { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<DebtorsList> DebtorsList { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Degree> Degree { get; set; }
        public DbSet<EducationalRecord> EducationalRecord { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<HostelBuilding> HostelBuilding { get; set; }
        public DbSet<HostelAllocation> HostelAllocation { get; set; }
        public DbSet<HostelRooms> HostelRooms { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<LGA> LGA { get; set; }
        public DbSet<NextOfKin> NextOfKin { get; set; }
        public DbSet<OtherDetails> OtherDetails { get; set; }

        // public DbSet<OLevelCredentials> OLevelCredentials { get; set; }
        public DbSet<ProgramChoice> ProgramChoice { get; set; }
        public DbSet<ProgramSetup> ProgramSetup { get; set; }
        public DbSet<Session> Session { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentAcademicQualification> StudentAcademicQualification { get; set; }
        public DbSet<StudentApplicationDocument> StudentApplicationDocument { get; set; }
        public DbSet<StudentMasterRecord> StudentMasterRecord { get; set; }
        public DbSet<StudentNextOfKin> StudentNextOfKin { get; set; }
        public DbSet<StudentOtherDetails> StudentOtherDetails { get; set; }
        public DbSet<StudentResult> StudentResult { get; set; }
        public DbSet<StudentResultUpload> StudentResultUpload { get; set; }
     
      
        public DbSet<HStudent> HStudent { get; set; }
        public DbSet<HStudentAcademicQualification> HStudentAcademicQualification { get; set; }
        public DbSet<HStudentApplicationDocument> HStudentApplicationDocument { get; set; }
        public DbSet<HStudentMasterRecord> HStudentMasterRecord { get; set; }
        public DbSet<HStudentNextOfKin> HStudentNextOfKin { get; set; }
        public DbSet<HStudentOtherDetails> HStudentOtherDetails { get; set; }
        public DbSet<HStudentResult> HStudentResult { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<SubjectSetup> SubjectSetup { get; set; }
        public DbSet<HealthAppointment> HealthAppointment { get; set; }
        public DbSet<PayCategory> PayCategory { get; set; }
        public DbSet<PayDetails> PayDetails { get; set; }
        public DbSet<PayConcession> PayConcession { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<AdmittedStudent> AdmittedStudent { get; set; }

         public DbSet<TransferForm> TransferForm { get; set; }

        public DbSet<PaymentLog> PaymentLog { get; set; }
        public DbSet<Semester> Semester { get; set; }
        public DbSet<PayUploadCategory> PayUploadCategory { get; set; }
        public DbSet<PayUploadCategoryBatch> PayUploadCategoryBatch { get; set; }
        public DbSet<PayUploadDetails> PayUploadDetails { get; set; }
        public DbSet<PayUploadConcession> PayUploadConcession { get; set; }
        public DbSet<StudentEnrolment> StudentEnrolment { get; set; }
        public DbSet<RemitaNotification> RemitaNotification { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure your customizations here
        }

    }
  
}

