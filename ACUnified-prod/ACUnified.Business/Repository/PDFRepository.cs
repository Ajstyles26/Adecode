
using ACUnified.Business.Repository.IRepository;
using ACUnified.Business.Util;
using ACUnified.Data.DTOs;
using ACUnified.Portal.Utils;



namespace ACUnified.Business.Repository;

public class PDFRepository:IPDFRepository
{

    public async Task<Stream> GenerateReceiptPDF(SchoolReceiptDTO receiptData, MemoryStream memoryStream)
    {
       
        // Write to memoryStream instead of file
        PDFReceiptGenerator.GenerateSchoolReceiptPdf(memoryStream, receiptData);
        //memoryStream.Position = 0;
        return  memoryStream;
    }

    public async Task<Stream> GenerateAdmissionLetterPDF(ApplicationFormDto applicationForm, LetterSettingsDto settings, MemoryStream memoryStream)
    {

        // Write to memoryStream instead of file
        PDFAdmissionLetterGenerator.GenerateAdmissionLetterPDF(memoryStream, applicationForm,settings);
        //memoryStream.Position = 0;
        return memoryStream;
    }

}