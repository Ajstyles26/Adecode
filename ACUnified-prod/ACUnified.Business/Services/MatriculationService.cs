using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACUnified.Business.Services
{
    public class MatriculationService : IMatriculationService
    {
        private readonly IApplicationFormRepository _applicationFormRepository;
        private readonly ILogger<MatriculationService> _logger;
        private readonly Dictionary<string, string> _programMatricPrefixes;

        public MatriculationService(
            IApplicationFormRepository applicationFormRepository,
            ILogger<MatriculationService> logger)
        {
            _applicationFormRepository = applicationFormRepository;
            _logger = logger;

            // Initialize program-to-prefix mapping
            _programMatricPrefixes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Agricultural Economics", "24AGR01" },
                { "Agricultural Extension", "24AGR01" },
                { "Crop Science", "24AGR02" },
                { "Animal Science", "24AGR02" },
                { "Environmental Health Science", "24BMS03" },
                { "Medical Laboratory Science", "24BMS01" },
                { "Nursing", "24BMS04" },
                { "Radiography and radiation Science", "24BMS02" },
                { "Mass Communication", "24S06" },
                { "Library and Information Science", "24S09" },
                { "Business Education", "24ED06" },
                { "Economics Education", "24ED05" },
                { "English Education", "24ED02" },
                { "Guidance and Counseling", "24ED01" },
                { "Computer Education", "24ED03" },
                { "Civil Engineering", "24EG04" },
                { "Computer Engineering", "24EG01" },
                { "Electrical Electronics Engineering", "24EG02" },
                { "Mechanical Engineering", "24EG03" },
                { "Architecture", "24EV01" },
                { "Estate Management", "24EV02" },
                { "English", "24H01" },
                { "History & International Studies", "24H02" },
                { "Performing Arts", "24H04" },
                { "Religious Studies", "24H03" },
                { "Law", "24L01" },
                { "Accounting", "24M01" },
                { "Banking and Finance", "24M03" },
                { "Business Administration", "24M04" },
                { "Industrial Relations and Personnel Management", "24M05" },
                { "Computer Science", "24N02" },
                { "Computer Science(ICT Option)", "24N04" },
                { "Industrial Chemistry", "24N03" },
                { "Physics", "24N07" },
                { "Geology", "24N05" },
                { "Microbiology", "24N06" },
                { "Biochemistry", "24N01" },
                { "Mathematics", "24N08" },
                { "Statistics", "24N09" },
                { "Economics", "24S05" },
                { "Political Science", "24S07" },
                { "Peace Studies and Conflict Resolution", "24S08" },
                { "Surveying and Geoinformatics", "24EV03" },
                { "Music", "24H05" },
                { "Entrepreneurship", "24M06" }
            };
        }

        /// <summary>
        /// Generates and saves matriculation numbers for all eligible applicants
        /// </summary>
        public async Task<int> GenerateAndSaveMatriculationNumbers()
        {
            try
            {
                // Get all applications that have reached stage 11 or above (matriculation stage)
                var applications = await _applicationFormRepository.GetApplicationsStage11AndAbove();
                int count = 0;

                // Group applications by finalized course
                var groupedApplications = applications
                    .Where(a => !string.IsNullOrEmpty(a.MatriculationNumber) == false) // Skip applications that already have matriculation numbers
                    .GroupBy(a => a.FinalisedCourse ?? (a.ProgramSetup?.Name ?? "Unknown"))
                    .ToDictionary(g => g.Key, g => g.ToList());

                // Process each program group
                foreach (var programGroup in groupedApplications)
                {
                    string programName = programGroup.Key;
                    var programApplications = programGroup.Value;

                    // Sort applications alphabetically by last name
                    var sortedApplications = programApplications
                        .OrderBy(a => a.BioData?.LastName ?? "")
                        .ToList();

                    // Generate matriculation numbers for the sorted applications
                    for (int i = 0; i < sortedApplications.Count; i++)
                    {
                        var application = sortedApplications[i];
                        
                        // Get the prefix for the program
                        if (!_programMatricPrefixes.TryGetValue(programName, out string prefix))
                        {
                            _logger.LogWarning($"No prefix defined for program: {programName}");
                            prefix = "24GEN01"; // Default prefix if not found
                        }

                        // Create sequential number based on sorted position
                        int sequenceNumber = i + 1;

                        // Check if this is a transfer degree
                        string suffix = "";
                        if (application.Degree != null && application.Degree.Name.Equals("TRANSFER", StringComparison.OrdinalIgnoreCase))
                        {
                            suffix = "TS";
                        }
                        
                        // Format with slash: [Prefix]/[3-digit sequence number][TS suffix for transfers]
                        string matricNumber = $"{prefix}/{sequenceNumber:D3}{suffix}";

                        // Update application with matriculation number
                        application.MatriculationNumber = matricNumber;
                        application.MatriculationDate = DateTime.Now;

                        // Save to database
                        await _applicationFormRepository.UpdateApplicationForm(application);
                        count++;
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating matriculation numbers");
                throw;
            }
        }

        /// <summary>
        /// Generates a matriculation number for a specific application
        /// </summary>
        public async Task<string> GenerateMatriculationNumber(ApplicationFormDto application)
        {
            try
            {
                if (application == null)
                {
                    _logger.LogWarning("Cannot generate matriculation number: Application is null");
                    return null;
                }

                // Get program name from the application
                string programName = application.FinalisedCourse;
                if (string.IsNullOrEmpty(programName) && application.ProgramSetup != null)
                {
                    programName = application.ProgramSetup.Name;
                }

                if (string.IsNullOrEmpty(programName))
                {
                    _logger.LogWarning($"Cannot generate matriculation number for application {application.Id}: Program name not found");
                    return null;
                }

                // Get the prefix for the program
                if (!_programMatricPrefixes.TryGetValue(programName, out string prefix))
                {
                    _logger.LogWarning($"No prefix defined for program: {programName}");
                    prefix = "24GEN01"; // Default prefix if not found
                }

                // Get all applications for this program that have reached matriculation stage (Stage 11+)
                var applications = await _applicationFormRepository.GetApplicationsByProgramNameForMatriculation(programName);
                
                // Filter applications without matriculation numbers
                var applicationsWithoutMatric = applications
                    .Where(a => string.IsNullOrEmpty(a.MatriculationNumber))
                    .ToList();
                
                // Sort applications alphabetically by last name
                var sortedApplications = applicationsWithoutMatric
                    .OrderBy(a => a.BioData?.LastName ?? "")
                    .ToList();
                
                // Find the position of the current application in the sorted list
                int position = sortedApplications.FindIndex(a => a.Id == application.Id);
                
                // If application not found in the list, use the count of applications that already have matriculation numbers
                if (position < 0)
                {
                    var existingMatricApplications = await _applicationFormRepository.GetApplicationsByMatriculationPrefix(prefix);
                    position = existingMatricApplications.Count();
                }
                
                // Sequence number is position + 1
                int sequenceNumber = position + 1;

                // Check if this is a transfer degree
                string suffix = "";
                if (application.Degree != null && application.Degree.Name.Equals("TRANSFER", StringComparison.OrdinalIgnoreCase))
                {
                    suffix = "TS";
                }
                
                // Format with slash: [Prefix]/[3-digit sequence number][TS suffix for transfers]
                return $"{prefix}/{sequenceNumber:D3}{suffix}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating matriculation number for application {application.Id}");
                return null;
            }
        }

        /// <summary>
        /// Generates a matriculation number for a specific application by ID
        /// </summary>
        public async Task<string> GenerateMatriculationNumberById(long applicationId)
        {
            try
            {
                var application = await _applicationFormRepository.GetApplicationForm(applicationId);
                if (application == null)
                {
                    _logger.LogWarning($"Application with ID {applicationId} not found");
                    return null;
                }

                var matricNumber = await GenerateMatriculationNumber(application);
                if (!string.IsNullOrEmpty(matricNumber))
                {
                    application.MatriculationNumber = matricNumber;
                    application.MatriculationDate = DateTime.Now;
                    await _applicationFormRepository.UpdateApplicationForm(application);
                }

                return matricNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating matriculation number for application ID {applicationId}");
                return null;
            }
        }
        

        /// <summary>
        /// Resets all matriculation numbers by setting MatriculationNumber to null and MatriculationDate to null
        /// </summary>
        public async Task<int> ResetAllMatriculationNumbers()
        {
            try
            {
                _logger.LogInformation("Starting reset of all matriculation numbers");
                
                // Get all applications that have matriculation numbers
                var applications = await _applicationFormRepository.GetApplicationsWithMatriculationNumbers();
                int count = 0;

                foreach (var application in applications)
                {
                    // Reset the matriculation data
                    application.MatriculationNumber = null;
                    application.MatriculationDate = null;

                    // Save to database
                    await _applicationFormRepository.UpdateApplicationForm(application);
                    count++;
                }

                _logger.LogInformation($"Successfully reset {count} matriculation numbers");
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting matriculation numbers");
                throw;
            }
        }
        public async Task<int> FixDuplicateMatriculationNumbers()
{
    try
    {
        // Fetch all applications with a matriculation number
        var allMatricApps = (await _applicationFormRepository.GetApplicationsWithMatriculationNumbers()).ToList();
        int fixedCount = 0;

        // Group by Program (FinalisedCourse or ProgramSetup.Name)
        var groupedByProgram = allMatricApps
            .GroupBy(a => a.FinalisedCourse ?? a.ProgramSetup?.Name ?? "Unknown")
            .ToList();

        foreach (var group in groupedByProgram)
        {
            var apps = group.OrderBy(a => a.BioData?.LastName ?? "").ThenBy(a => a.BioData?.FirstName ?? "").ToList();

            // Map MatricNumber to list of applications
            var duplicates = apps
                .GroupBy(a => a.MatriculationNumber)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Skip(1)) // skip the first, keep others
                .ToList();

            if (duplicates.Count > 0)
            {
                // Generate all matric numbers for this group (force re-sequence)
                for (int i = 0; i < apps.Count; i++)
                {
                    var app = apps[i];

                    // Get prefix
                    if (!_programMatricPrefixes.TryGetValue(group.Key, out string prefix))
                        prefix = "24GEN01";
                    
                    int sequenceNumber = i + 1;
                    string suffix = "";
                    if (app.Degree != null && app.Degree.Name.Equals("TRANSFER", StringComparison.OrdinalIgnoreCase))
                        suffix = "TS";
                    
                    string newMatricNumber = $"{prefix}/{sequenceNumber:D3}{suffix}";

                    // If duplicate or not matching the correct sequence, reassign
                    if (app.MatriculationNumber != newMatricNumber)
                    {
                        app.MatriculationNumber = newMatricNumber;
                        app.MatriculationDate = DateTime.Now;
                        await _applicationFormRepository.UpdateApplicationForm(app);
                        fixedCount++;
                    }
                }
            }
        }
        return fixedCount;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error fixing duplicate matriculation numbers");
        throw;
    }
}

        /// <summary>
        /// Gets all program prefixes for admin UI
        /// </summary>
        public Dictionary<string, string> GetAllProgramPrefixes()
        {
            return new Dictionary<string, string>(_programMatricPrefixes, StringComparer.OrdinalIgnoreCase);
        }
    }
}