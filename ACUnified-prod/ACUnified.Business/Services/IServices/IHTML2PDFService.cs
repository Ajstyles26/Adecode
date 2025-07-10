using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IHTML2PDFService
    {
        public byte[] GeneratePDF(string htmlfile);
        
    }
}
