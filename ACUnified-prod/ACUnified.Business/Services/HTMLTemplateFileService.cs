using ACUnified.Business.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ACUnified.Business.Services
{
    public class HTMLTemplateFileService : IHTMLTemplateFileService
    {
        public readonly IWebHostEnvironment _webHostEnvironment;
        public HTMLTemplateFileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> GetTemplateFileAsync(string filepath)
        {
           string fileloc=Path.Combine(_webHostEnvironment.ContentRootPath,filepath);
            return await File.ReadAllTextAsync(fileloc);
        }

        public async Task SaveTemplateFileAsync(string filepath, string content)
        {
            var fileloc = Path.Combine(_webHostEnvironment.ContentRootPath, filepath);
            var directory = Path.GetDirectoryName(fileloc);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
            await File.WriteAllTextAsync(fileloc, content);
        }
    }
}
