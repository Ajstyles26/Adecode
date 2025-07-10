using ACUnified.Data.DTOs;

namespace ACUnified.Business.Repository.IRepository;

public interface IPDFRepository
{
    Task<Stream> GenerateReceiptPDF(SchoolReceiptDTO receiptData, MemoryStream memoryStream);
    Task<Stream> GenerateAdmissionLetterPDF(ApplicationFormDto applicationForm, LetterSettingsDto settings, MemoryStream memoryStream);
}