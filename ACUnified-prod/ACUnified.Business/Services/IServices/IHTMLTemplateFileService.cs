using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACUnified.Business.Services.IServices
{
    public interface IHTMLTemplateFileService
    {
       Task<string> GetTemplateFileAsync(string filepath);
       Task SaveTemplateFileAsync(string filepath, string content);
    }
}
