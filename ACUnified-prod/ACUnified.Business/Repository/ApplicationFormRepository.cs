using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Services;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;

using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZXing;

namespace ACUnified.Business.Repository
{
    public class ApplicationFormRepository : IApplicationFormRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> dbOptions;
        private readonly IMapper _mapper;
        public ApplicationFormRepository(DbContextOptions<ApplicationDbContext> options, IMapper mapper)
        {
            _mapper = mapper;
            dbOptions = options;
        }

        public async Task<ApplicationFormDto> CreateApplicationForm(ApplicationFormDto ApplicationFormDto)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                ApplicationForm ApplicationForm = _mapper.Map<ApplicationFormDto, ApplicationForm>(ApplicationFormDto);
                var addedApplicationForm = db.ApplicationForm.Add(ApplicationForm);
                await db.SaveChangesAsync();
                //Verify if the data exist in biodata if yes update if no just save existing

                return _mapper.Map<ApplicationForm, ApplicationFormDto>(addedApplicationForm.Entity);
            }
        }
         public async Task<IEnumerable<ApplicationFormDto>> GetAll()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var applicationForms = await db.ApplicationForm
                    .Include(x => x.ApplicationUser)
                        .Include(x => x.BioData)
                        .Include(x => x.ProgramSetup)
                    .ToListAsync();
                
                return _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(applicationForms);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetAll: {ex.Message}");
            return new List<ApplicationFormDto>();
        }
    }
        //REGULAR
        public async Task<IEnumerable<ApplicationFormDto>> GetAllApplicationForm()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Include(x=>x.BioData)
                        .Include(x=>x.OtherDetails)
                        .Include(x=>x.AcademicQualification)
                         .Include(x=>x.NextOfKin)
                          .Include(x=>x.References)
                          .Include(x=>x.Degree)
                          .Where(y=>y.Degree.Name=="BSC"));
                        

                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetCompletedApplicationForm()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage3)
                        .Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References)
                        .Include(x => x.Degree)
                        .Where(y => y.Degree.Name == "BSC" || y.Degree.Name == "TRANSFER");

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetAuthorizedApplicationForm()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Include(x => x.BioData)
                        .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage4)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "BSC" || y.Degree.Name == "TRANSFER"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetFinalizedApplicationForm()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5).Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "BSC" || y.Degree.Name == "TRANSFER"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<ApplicationFormRankingsDto>> GetApplicationFormsByAppliedCourses()
        {
            IEnumerable<ApplicationFormRankingsDto> applicationFormRankings = new List<ApplicationFormRankingsDto>();

            using (var db = new ApplicationDbContext(dbOptions))
            {
                applicationFormRankings = db.ApplicationForm
                .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage3)
                .Join(
                    db.AcademicQualification,
                    x => x.AcademicQualificationId,
                    aq => aq.Id,
                    (x, aq) => new { aq.Choice1, aq.Id }
                )
                .GroupBy(dt => dt.Choice1)
                .Select(g => new ApplicationFormRankingsDto
                {
                    Choice1 = g.Key,
                    Count = g.Count()
                }).ToList();
            }
            return applicationFormRankings;
        }

         public async Task<IEnumerable<ApplicationFormRankingsDto>> GetAdmittedStudentsReg()
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            return await db.ApplicationForm
                .Include(x => x.Degree)
                .Where(y => y.Degree != null && y.Degree.Name == "BSC")
                .Where(x => x.ApplicantStage >= Data.Enum.ApplicationStage.Stage5)
                .Where(x => x.FinalisedCourse != null)
                .GroupBy(af => af.FinalisedCourse)
                .Select(g => new ApplicationFormRankingsDto
                {
                    Choice1 = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"Error in GetAdmittedStudentsReg: {ex.Message}");
        // You might want to throw a custom exception here or return an empty list
        return new List<ApplicationFormRankingsDto>();
    }
}

 public async Task<IEnumerable<ApplicationFormRankingsDto>> GetPaidfee()
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            return await db.ApplicationForm
                .Include(x => x.Degree)
                .Where(y => y.Degree != null && y.Degree.Name == "BSC")
                .Where(x => x.ApplicantStage >= Data.Enum.ApplicationStage.Stage6)
                .Where(x => x.FinalisedCourse != null)
                .GroupBy(af => af.FinalisedCourse)
                .Select(g => new ApplicationFormRankingsDto
                {
                    Choice1 = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"Error in GetAdmittedStudentsReg: {ex.Message}");
        // You might want to throw a custom exception here or return an empty list
        return new List<ApplicationFormRankingsDto>();
    }
}

public async Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudentsDetailsReg()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Include(x => x.BioData)
                        .Include(x => x.Degree)
                        .Where(y => y.Degree.Name == "BSC")
                        .Where(x => x.ApplicantStage >= Data.Enum.ApplicationStage.Stage5)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References);

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudents()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Include(x => x.BioData)
                        .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References);

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //REGULAR

        //BTH
        public async Task<IEnumerable<ApplicationFormDto>> GetAllApplicationFormBTH()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "BTHBA"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<ApplicationFormDto>> GetRecentlyGeneratedMatriculationNumbers(int count = 10)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var recentApplications = await db.ApplicationForm
                        .Include(x => x.BioData)
                        .Include(x => x.ProgramSetup)
                        .Where(x => x.MatriculationNumber != null && x.MatriculationDate != null)
                        .OrderByDescending(x => x.MatriculationDate)
                        .Take(count)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(recentApplications);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRecentlyGeneratedMatriculationNumbers: {ex.Message}");
                return new List<ApplicationFormDto>();
            }
        }

        // Check if an application exists and is eligible for matriculation
        public async Task<bool> IsApplicationEligibleForMatriculation(long applicationId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var application = await db.ApplicationForm
                        .Include(x => x.Degree)
                        .FirstOrDefaultAsync(x => x.Id == applicationId);

                    if (application == null)
                        return false;

                    // Check eligibility criteria
                    bool isEligible = application.ApplicantStage >= Data.Enum.ApplicationStage.Stage11 &&
                                      string.IsNullOrEmpty(application.MatriculationNumber) &&
                                      application.Degree != null &&
                                      (application.Degree.Name == "BSC" || application.Degree.Name == "TRANSFER");

                    return isEligible;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IsApplicationEligibleForMatriculation: {ex.Message}");
                return false;
            }
        }

        // Get pending matriculation applications (eligible but not yet assigned)
        public async Task<IEnumerable<ApplicationFormDto>> GetPendingMatriculationApplications()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var pendingApplications = await db.ApplicationForm
                        .Include(x => x.BioData)
                        .Include(x => x.Degree)
                        .Include(x => x.ProgramSetup)
                        .Where(x => x.ApplicantStage >= Data.Enum.ApplicationStage.Stage11 &&
                                   x.MatriculationNumber == null &&
                                   x.Degree != null &&
                                   (x.Degree.Name == "BSC" || x.Degree.Name == "TRANSFER"))
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(pendingApplications);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPendingMatriculationApplications: {ex.Message}");
                return new List<ApplicationFormDto>();
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetCompletedApplicationFormBTH()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage3)
                        .Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References)
                        .Include(x => x.Degree)
                        .Where(y => y.Degree.Name == "BTHBA");

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetAuthorizedApplicationFormBTH()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Include(x => x.BioData)
                        .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage4)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "BTHBA"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetFinalizedApplicationFormBTH()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5).Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "BTHBA"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormRankingsDto>> GetApplicationFormsByAppliedCoursesBTH()
        {
            IEnumerable<ApplicationFormRankingsDto> applicationFormRankings = new List<ApplicationFormRankingsDto>();

            using (var db = new ApplicationDbContext(dbOptions))
            {
                applicationFormRankings = db.ApplicationForm.Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "BTHBA")
                .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage3)
                .Join(
                    db.AcademicQualification,
                    x => x.AcademicQualificationId,
                    aq => aq.Id,
                    (x, aq) => new { aq.Choice1, aq.Id }
                )
                .GroupBy(dt => dt.Choice1)
                .Select(g => new ApplicationFormRankingsDto
                {
                    Choice1 = g.Key,
                    Count = g.Count()
                }).ToList();
            }
            return applicationFormRankings;
        }

    public async Task<IEnumerable<ApplicationFormRankingsDto>> GetAdmittedStudentsBTH()
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            return await db.ApplicationForm
                .Include(x => x.Degree)
                .Where(y => y.Degree != null && y.Degree.Name == "BTHBA")
                .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5)
                .Where(x => x.FinalisedCourse != null)
                .GroupBy(af => af.FinalisedCourse)
                .Select(g => new ApplicationFormRankingsDto
                {
                    Choice1 = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"Error in GetAdmittedStudentsBTH: {ex.Message}");
        // You might want to throw a custom exception here or return an empty list
        return new List<ApplicationFormRankingsDto>();
    }
}

                
        public async Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudentsDetailsBTH()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Include(x => x.BioData)
                        .Include(x => x.Degree)
                        .Where(y => y.Degree.Name == "BTHBA")
                        .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References);

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //BTH
        //JUPEB  
          public async Task<IEnumerable<ApplicationFormDto>> GetAllApplicationFormJUPEB()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "JUPEB"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetCompletedApplicationFormJUPEB()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage3)
                        .Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References)
                        .Include(x => x.Degree)
                        .Where(y => y.Degree.Name == "JUPEB");

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetAuthorizedApplicationFormJUPEB()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Include(x => x.BioData)
                        .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage4)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "JUPEB"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
public async Task<string> GetLastUsedNumber()
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var lastFormRefNo = await db.ApplicationForm
                .Where(a => a.FormRefNo.Contains("SP"))
                .OrderByDescending(a => a.FormRefNo)
                .Select(a => a.FormRefNo)
                .FirstOrDefaultAsync();

            if (lastFormRefNo != null && lastFormRefNo.Length > 6)
            {
                var numericPart = lastFormRefNo.Substring(lastFormRefNo.IndexOf("SP") + 6);
                return numericPart;
            }

            return "0009"; // Return "0009" so that the first generated number will be "0010"
        }
    }
    catch (Exception ex)
    {
        // Consider logging the exception here
        return "0009"; // Return default value in case of an exception
    }
}
        public async Task<IEnumerable<ApplicationFormDto>> GetFinalizedApplicationFormJUPEB()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5).Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "BTHBA"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormRankingsDto>> GetApplicationFormsByAppliedCoursesJUPEB()
        {
            IEnumerable<ApplicationFormRankingsDto> applicationFormRankings = new List<ApplicationFormRankingsDto>();

            using (var db = new ApplicationDbContext(dbOptions))
            {
                applicationFormRankings = db.ApplicationForm.Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "JUPEB")
                .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage3)
                .Join(
                    db.AcademicQualification,
                    x => x.AcademicQualificationId,
                    aq => aq.Id,
                    (x, aq) => new { aq.Choice1, aq.Id }
                )
                .GroupBy(dt => dt.Choice1)
                .Select(g => new ApplicationFormRankingsDto
                {
                    Choice1 = g.Key,
                    Count = g.Count()
                }).ToList();
            }
            return applicationFormRankings;
        }

    public async Task<IEnumerable<ApplicationFormRankingsDto>> GetAdmittedStudentsJUPEB()
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            return await db.ApplicationForm
                .Include(x => x.Degree)
                .Where(y => y.Degree != null && y.Degree.Name == "JUPEB")
                .Where(x => x.ApplicantStage >= Data.Enum.ApplicationStage.Stage5)
                .Where(x => x.FinalisedCourse != null)
                .GroupBy(af => af.FinalisedCourse)
                .Select(g => new ApplicationFormRankingsDto
                {
                    Choice1 = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"Error in GetAdmittedStudentsBTH: {ex.Message}");
        // You might want to throw a custom exception here or return an empty list
        return new List<ApplicationFormRankingsDto>();
    }
}

                
        public async Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudentsDetailsJUPEB()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Include(x => x.BioData)
                        .Include(x => x.Degree)
                        .Where(y => y.Degree.Name == "JUPEB")
                        .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References);

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        

        //PG
        public async Task<IEnumerable<ApplicationFormDto>> GetAllApplicationFormPG()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x => x.Degree)
                          .Where(y => y.Degree.Name == "MSC" || y.Degree.Name == "PHD" || y.Degree.Name == "PGD" || y.Degree.Name == "MBA" || y.Degree.Name == "DBA" || y.Degree.Name == "MA"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetCompletedApplicationFormPG()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage3)
                        .Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References)
                        .Where(y => y.Degree.Name == "MSC" || y.Degree.Name == "PHD" || y.Degree.Name == "PGD" || y.Degree.Name == "MBA" || y.Degree.Name == "DBA" || y.Degree.Name == "MA");

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetAuthorizedApplicationFormPG()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Include(x => x.BioData)
                        .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage4)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                            .Where(y => y.Degree.Name == "MSC" || y.Degree.Name == "PHD" || y.Degree.Name == "PGD" || y.Degree.Name == "MBA" || y.Degree.Name == "DBA" || y.Degree.Name == "MA"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetFinalizedApplicationFormPG()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(db.ApplicationForm.Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5).Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                          .Include(x => x.References)
                          .Include(x=>x.Degree)
                          .Where(y => y.Degree.Name == "MSC" || y.Degree.Name == "PHD" || y.Degree.Name == "PGD" || y.Degree.Name == "MBA" || y.Degree.Name == "DBA" || y.Degree.Name == "MA"));


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<ApplicationFormRankingsDto>> GetApplicationFormsByAppliedCoursesPG()
        {
            IEnumerable<ApplicationFormRankingsDto> applicationFormRankings = new List<ApplicationFormRankingsDto>();

            using (var db = new ApplicationDbContext(dbOptions))
            {
                applicationFormRankings = db.ApplicationForm.Include(x=>x.Degree)
                .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage3)
                .Where(y => y.Degree.Name == "MSC" || y.Degree.Name == "PHD" || y.Degree.Name == "PGD" || y.Degree.Name == "MBA" || y.Degree.Name == "DBA" || y.Degree.Name == "MA")
                .Join(
                    db.AcademicQualification,
                    x => x.AcademicQualificationId,
                    aq => aq.Id,
                    (x, aq) => new { aq.Choice1, aq.Id }
                )
                .GroupBy(dt => dt.Choice1)
                .Select(g => new ApplicationFormRankingsDto
                {
                    Choice1 = g.Key,
                    Count = g.Count()
                }).ToList();
            }
            return applicationFormRankings;
        }

        public async Task<IEnumerable<ApplicationFormDto>> GetAdmittedStudentsPG()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var activeSessionId = await db.Session.Where(s => s.isApplicantActive)
                                                          .Select(s => (long?)s.Id)
                                                          .FirstOrDefaultAsync();

                    IQueryable<ApplicationForm> query = db.ApplicationForm.Include(x => x.BioData)
                        .Where(x => x.ApplicantStage == Data.Enum.ApplicationStage.Stage5)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References)
                        .Include(x => x.Degree)
                        .Where(y => y.Degree.Name == "MSC" || y.Degree.Name == "PHD" || y.Degree.Name == "PGD" || y.Degree.Name == "MBA" || y.Degree.Name == "DBA" || y.Degree.Name == "MA");

                    if (activeSessionId != null)
                    {
                        query = query.Where(x => x.SessionId == activeSessionId);
                    }

                    IEnumerable<ApplicationFormDto> ApplicationFormDtos =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(query);


                    return ApplicationFormDtos;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //PG




        public async Task<ApplicationFormDto> GetApplicationForm(long Id)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    ApplicationFormDto ApplicationFormDto =
                        _mapper.Map<ApplicationForm, ApplicationFormDto>(await db.ApplicationForm.FirstOrDefaultAsync(x => x.Id == Id));
                    return ApplicationFormDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ApplicationFormDto> GetApplicationFormByUserId(string userId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var data = db.ApplicationForm.Where(x => x.UserId == userId)
                        .Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References)
                        .Include(x => x.ProgramSetup)
                        .Include(x => x.TransferForm)
                        .FirstOrDefault();
                        

                    ApplicationFormDto ApplicationFormDto =
                        _mapper.Map<ApplicationForm, ApplicationFormDto>(data);


              
                    return  ApplicationFormDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
public async Task<ApplicationFormDto> GetCompletedApplicationForm(string userId)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var data = db.ApplicationForm.Where(x => x.UserId == userId)
                        .Include(x => x.BioData)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.NextOfKin)
                        .Include(x => x.References)
                        .Include(x => x.TransferForm)
                        .Include(x => x.ProgramSetup)
                        
                        .ThenInclude(p => p.Department)
                        .Include(x => x.ProgramSetup)
                     .ThenInclude(p => p.Faculty)
                        .FirstOrDefault();
                        

                    ApplicationFormDto ApplicationFormDto =
                        _mapper.Map<ApplicationForm, ApplicationFormDto>(data);


              
                    return  ApplicationFormDto;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task DeleteApplicationForm(long Id)
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var ApplicationForm = db.ApplicationForm.FirstOrDefault(x => x.Id == Id);
                if (ApplicationForm != null)
                {
                    db.ApplicationForm.Remove(ApplicationForm);
                    db.SaveChanges();
                }


            }
        }


//stats
// Stage 1: BSC Applications - Not Paid
    public async Task<IEnumerable<ApplicationFormDto>> GetBScNotPaidApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage1 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScNotPaidApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 2: BSC Applications - Paid but not submitted
    public async Task<IEnumerable<ApplicationFormDto>> GetBScPaidNotSubmittedApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage2 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScPaidNotSubmittedApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 3: BSC Applications - Submitted
    public async Task<IEnumerable<ApplicationFormDto>> GetBScSubmittedApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Include(x => x.OtherDetails)
                            .Include(x => x.AcademicQualification)
                            .Include(x => x.NextOfKin)
                            .Include(x => x.References)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage3 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScSubmittedApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 4: BSC Applications - Decision Made
    public async Task<IEnumerable<ApplicationFormDto>> GetBScDecisionMadeApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Include(x => x.OtherDetails)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage4 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScDecisionMadeApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 5: BSC Applications - Provisional Admission
    public async Task<IEnumerable<ApplicationFormDto>> GetBScProvisionalAdmissionApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Include(x => x.OtherDetails)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage5 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScProvisionalAdmissionApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 6: BSC Applications - Acceptance Fee Paid
    public async Task<IEnumerable<ApplicationFormDto>> GetBScAcceptanceFeePaidApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Include(x => x.OtherDetails)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage6 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScAcceptanceFeePaidApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 7: BSC Applications - School Fees Paid
    public async Task<IEnumerable<ApplicationFormDto>> GetBScSchoolFeesPaidApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Include(x => x.OtherDetails)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage7 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScSchoolFeesPaidApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 8: BSC Applications - Hostel Selection In Progress
    public async Task<IEnumerable<ApplicationFormDto>> GetBScHostelSelectionInProgressApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage8 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScHostelSelectionInProgressApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 9: BSC Applications - Hostel Selected
    public async Task<IEnumerable<ApplicationFormDto>> GetBScHostelSelectedApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage9 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScHostelSelectedApplications: {ex.Message}");
            return null;
        }
    }

    // Stage 10: BSC Applications - Medical Fee Paid
    public async Task<IEnumerable<ApplicationFormDto>> GetBScMedicalFeePaidApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage10 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScMedicalFeePaidApplications: {ex.Message}");
            return null;
        }
    }
    public async Task<IEnumerable<ApplicationFormDto>> GetApplicationsWithMatriculationNumbers()
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            IEnumerable<ApplicationFormDto> applications =
                _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                    await db.ApplicationForm
                          .Include(x => x.BioData)
                          .Include(x => x.Level)
                        .Include(x => x.Session)
                        .Include(x => x.Degree)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.ProgramSetup)
                        .ThenInclude(p => p.Department)
                        .Include(x => x.ProgramSetup)
                        .ThenInclude(p => p.Faculty)
                        .Where(x => !string.IsNullOrEmpty(x.MatriculationNumber))
                        .ToListAsync()
                );
            return applications;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GetApplicationsWithMatriculationNumbers: {ex.Message}");
        return null;
    }
}
 //Stage 11: BSC Applications - Matriculation Fee Paid
    public async Task<IEnumerable<ApplicationFormDto>> GetBScMatriculationFeePaidApplications()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                IEnumerable<ApplicationFormDto> applications =
                    _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                        await db.ApplicationForm
                            .Include(x => x.Degree)
                            .Include(x => x.BioData)
                            .Where(x => x.ApplicantStage == ApplicationStage.Stage11 && 
                                      x.Degree.Name == "BSC")
                            .ToListAsync()
                    );
                return applications;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScMatriculationFeePaidApplications: {ex.Message}");
            return null;
        }
    }
  // Get Applications at Stage 11 and above
// Get Applications at Stage 11 and above

public async Task<IEnumerable<ApplicationFormDto>> GetApplicationsByProgramNameForMatriculation(string programName)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            IEnumerable<ApplicationFormDto> applications =
                _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                    await db.ApplicationForm
                        .Include(x => x.BioData)
                        .Include(x => x.Degree)
                        .Include(x => x.ProgramSetup)
                        .Where(x => x.ApplicantStage >= ApplicationStage.Stage11 && 
                               (x.Degree.Name == "BSC" || x.Degree.Name == "TRANSFER") &&
                               (x.FinalisedCourse == programName || 
                               (x.ProgramSetup != null && x.ProgramSetup.Name == programName)))
                        .ToListAsync()
                );
            return applications;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GetApplicationsByProgramNameForMatriculation: {ex.Message}");
        return new List<ApplicationFormDto>();
    }
}
public async Task<IEnumerable<ApplicationFormDto>> GetApplicationsStage11AndAbove()
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            IEnumerable<ApplicationFormDto> applications =
                _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                    await db.ApplicationForm
                        .Include(x => x.BioData)
                          .Include(x => x.Level)
                        .Include(x => x.Session)
                        .Include(x => x.Degree)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.ProgramSetup)
                        .ThenInclude(p => p.Department)
                        .Include(x => x.ProgramSetup)
                        .ThenInclude(p => p.Faculty)
                        .Where(x => x.ApplicantStage >= ApplicationStage.Stage7 && 
                               (x.Degree.Name == "BSC" || x.Degree.Name == "TRANSFER"))
                        .ToListAsync()
                );
            return applications;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GetApplicationsStage11AndAbove: {ex.Message}");
        return new List<ApplicationFormDto>();
    }
}

   public async Task<IEnumerable<ApplicationFormDto>> GetApplicationsByMatriculationPrefix(string prefix)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    IEnumerable<ApplicationFormDto> applicationForms =
                        _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(
                            await db.ApplicationForm
                                .Where(x => x.MatriculationNumber != null && x.MatriculationNumber.StartsWith(prefix))
                                .ToListAsync());
                    
                    return applicationForms;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetApplicationsByMatriculationPrefix: {ex.Message}");
                return new List<ApplicationFormDto>();
            }
        }
        
        // Method to update ApplicationFormDto with the matriculation number field
        public async Task<bool> UpdateMatriculationNumber(long applicationId, string matriculationNumber)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var application = await db.ApplicationForm.FindAsync(applicationId);
                    if (application == null)
                        return false;
                        
                    application.MatriculationNumber = matriculationNumber;
                    application.MatriculationDate = DateTime.Now;
                    
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateMatriculationNumber: {ex.Message}");
                return false;
            }
        }

public async Task<ApplicationFormDto> GetBScApplicationFormById(long id)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var application = await db.ApplicationForm
                .Include(x => x.BioData)
                          .Include(x => x.Level)
                        .Include(x => x.Session)
                        .Include(x => x.Degree)
                        .Include(x => x.AcademicQualification)
                         .Include(x => x.NextOfKin)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.ProgramSetup)
                        .ThenInclude(p => p.Department)
                        .Include(x => x.ProgramSetup)
                        .ThenInclude(p => p.Faculty)
                .FirstOrDefaultAsync(x => x.Id == id && x.Degree.Name == "BSC");

            return _mapper.Map<ApplicationForm, ApplicationFormDto>(application);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in GetBScApplicationFormById: {ex.Message}");
        return null;
    }
}

public async Task<ApplicationFormDto> UpdateApplicationStage(ApplicationForm updatedApplication)
{
    try
    {
        using (var db = new ApplicationDbContext(dbOptions))
        {
            var existingApplication = await db.ApplicationForm
                .Include(x => x.BioData)
                .Include(x => x.OtherDetails)
                .Include(x => x.AcademicQualification)
                .Include(x => x.NextOfKin)
                .Include(x => x.References)
                .Include(x => x.Degree)
                .FirstOrDefaultAsync(x => x.Id == updatedApplication.Id);

            if (existingApplication == null)
                return null;

            // Update only the stage-related fields
            existingApplication.ApplicantStage = updatedApplication.ApplicantStage;
            existingApplication.Decision = updatedApplication.Decision;
            existingApplication.DecisionComment = updatedApplication.DecisionComment;
            existingApplication.DecisionDate = updatedApplication.DecisionDate;
            existingApplication.ApprovedCourse = updatedApplication.ApprovedCourse;
            existingApplication.FinalisedCourse = updatedApplication.FinalisedCourse;
            existingApplication.FinalisedDecision = updatedApplication.FinalisedDecision;
            existingApplication.FinalisedComment = updatedApplication.FinalisedComment;
            existingApplication.FinalDecisionDate = updatedApplication.FinalDecisionDate;
            existingApplication.DecisionMakerUserId = updatedApplication.DecisionMakerUserId;
            existingApplication.FinalizedUserId = updatedApplication.FinalizedUserId;

            await db.SaveChangesAsync();

            return _mapper.Map<ApplicationForm, ApplicationFormDto>(existingApplication);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in UpdateApplicationStage: {ex.Message}");
        return null;
    }
}

    // BSC Statistics
    public async Task<Dictionary<ApplicationStage, int>> GetBScApplicationStatistics()
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                var statistics = await db.ApplicationForm
                    .Include(x => x.Degree)
                    .Where(x => x.Degree.Name == "BSC")
                    .GroupBy(x => x.ApplicantStage)
                    .Select(g => new { Stage = g.Key, Count = g.Count() })
                    .ToDictionaryAsync(x => x.Stage, x => x.Count);

                return statistics;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScApplicationStatistics: {ex.Message}");
            return new Dictionary<ApplicationStage, int>();
        }
    }

    // Get BSC Applications Count by Stage
    public async Task<int> GetBScApplicationCountByStage(ApplicationStage stage)
    {
        try
        {
            using (var db = new ApplicationDbContext(dbOptions))
            {
                return await db.ApplicationForm
                    .Include(x => x.Degree)
                    .Where(x => x.ApplicantStage == stage && 
                               x.Degree.Name == "BSC")
                    .CountAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetBScApplicationCountByStage: {ex.Message}");
            return 0;
        }
    }

    public async Task<IEnumerable<ApplicationFormDto>> GetApplicationsStage11AndAboves()
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    var applications = await db.ApplicationForm
                        .Include(x => x.BioData)
                        .Include(x => x.Degree)
                        .Include(x => x.OtherDetails)
                        .Include(x => x.AcademicQualification)
                        .Include(x => x.Session)
                        .Include(x => x.Level)
                        .Include(x => x.ProgramSetup)
                            .ThenInclude(p => p.Department)
                        .Include(x => x.ProgramSetup)
                            .ThenInclude(p => p.Faculty)
                        .Where(x => x.ApplicantStage >= ApplicationStage.Stage7 && 
                                   x.Degree != null && 
                                   (x.Degree.Name == "BSC" || x.Degree.Name == "TRANSFER"))
                        .ToListAsync();

                    var dtos = _mapper.Map<IEnumerable<ApplicationForm>, IEnumerable<ApplicationFormDto>>(applications);
                    Console.WriteLine($"GetApplicationsStage11AndAbove: Fetched {applications.Count} applications");
                    return dtos ?? new List<ApplicationFormDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetApplicationsStage11AndAboves: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return new List<ApplicationFormDto>();
            }
        }

        public async Task<long> AddAdmittedStudent(AdmittedStudent admittedStudent)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    db.AdmittedStudent.Add(admittedStudent);
                    await db.SaveChangesAsync();
                    return admittedStudent.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddAdmittedStudent: {ex.Message}");
                return -1;
            }
        }
        //ens
        public async Task<ApplicationFormDto> UpdateApplicationForm(ApplicationFormDto ApplicationFormDto)
        {
            try
            {
                using (var db = new ApplicationDbContext(dbOptions))
                {
                    {

                        ApplicationForm ApplicationForm = db.ApplicationForm.FirstOrDefault(x => x.Id == ApplicationFormDto.Id);
                        if (ApplicationForm == null)
                        {
                            return null;
                        }
                        else
                        {
                            ApplicationForm ApplicationFormUpdate = _mapper.Map<ApplicationFormDto, ApplicationForm>(ApplicationFormDto, ApplicationForm);
                            var currentupdate = db.ApplicationForm.Update(ApplicationFormUpdate);
                            //Verify if the data exist in biodata if yes update if no just save existing
                            await db.SaveChangesAsync();
                            return _mapper.Map<ApplicationForm, ApplicationFormDto>(currentupdate.Entity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

     
    }

}
