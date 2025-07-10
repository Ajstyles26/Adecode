using ACUnified.Business.Services.IServices;
using iText.Html2pdf;
using iText.Kernel.Pdf.Navigation;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services
{
    public class HTML2PDFService : IHTML2PDFService
    {
        public byte[] GeneratePDF(string htmlContent)
{
    try
    {
        using (MemoryStream ms = new MemoryStream())
        {
            ConverterProperties converterProperties = new ConverterProperties();
            // Add any necessary converter properties

            HtmlConverter.ConvertToPdf(htmlContent, ms, converterProperties);
            return ms.ToArray();
        }
    }
    catch (Exception ex)
    {
        // Log the full exception details
        Console.WriteLine($"PDF Generation Error: {ex}");
        
        // Log the HTML content that caused the error
        Console.WriteLine($"Problematic HTML: {htmlContent}");
        
        // Rethrow or handle the exception as needed
        throw;
    }
}
    }
}
