using ACUnified.Data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IMatriculationService
    {
        /// <summary>
        /// Generates and saves matriculation numbers for all eligible applicants
        /// </summary>
        Task<int> GenerateAndSaveMatriculationNumbers();

        /// <summary>
        /// Generates a matriculation number for a specific application
        /// </summary>
        Task<string> GenerateMatriculationNumber(ApplicationFormDto application);

        /// <summary>
        /// Generates a matriculation number for a specific application by ID
        /// </summary>
        Task<string> GenerateMatriculationNumberById(long applicationId);

        /// <summary>
        /// Resets all matriculation numbers by setting MatriculationNumber to null and MatriculationDate to null
        /// </summary>
        Task<int> ResetAllMatriculationNumbers();
        Task<int> FixDuplicateMatriculationNumbers();

        /// <summary>
        /// Gets all program prefixes for admin UI
        /// </summary>
        Dictionary<string, string> GetAllProgramPrefixes();
    }
}