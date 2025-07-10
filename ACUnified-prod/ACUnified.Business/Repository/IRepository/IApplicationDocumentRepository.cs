using ACUnified.Data.DTOs;
using ACUnified.Data.Enum;



namespace ACUnified.Business.Repository.IRepository
{
    public interface IApplicationDocumentRepository
    {
        Task<ApplicationDocumentDto> CreateApplicationDocument(ApplicationDocumentDto ApplicationDocumentDto);
        Task<IEnumerable<ApplicationDocumentDto>> GetAllApplicationDocument();
        Task<ApplicationDocumentDto> GetApplicationDocument(long Id);
          Task<ApplicationDocumentDto> Getdocumentbytypeanduserid(DocumentType type, string userid);
        Task DeleteApplicationDocument(long Id);
        Task<ApplicationDocumentDto> UpdateApplicationDocument(ApplicationDocumentDto ApplicationDocumentDto);
        Task<IEnumerable<ApplicationDocumentDto>> GetApplicationDocumentByUserId(string userId);
    }

}
