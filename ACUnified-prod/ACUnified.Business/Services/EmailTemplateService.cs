using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACUnified.Portal.Data;
using Microsoft.Extensions.Hosting;
using static QRCoder.PayloadGenerator;

namespace ACUnified.Business.Services.IServices
{
    

    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IHostEnvironment _environment;
        private readonly Dictionary<string, string> _templateCache;

        public EmailTemplateService(IHostEnvironment environment)
        {
            _environment = environment;
            _templateCache = new Dictionary<string, string>();
        }

        public string GetWelcomeTemplate(string userName)
        {
            var template = GetTemplate("Welcome.html");
            return template.Replace("{{UserName}}", userName);
        }

        public string GetPasswordResetTemplate(string resetLink, string userName)
        {
            var template = GetTemplate("PasswordReset.html");
            return template
                .Replace("{{UserName}}", userName)
                .Replace("{{ResetLink}}", resetLink);
        }

        public string GetNotificationTemplate(string subject, string message)
        {
            var template = GetTemplate("Notification.html");
            return template
                .Replace("{{Subject}}", subject)
                .Replace("{{Message}}", message);
        }

        private string GetTemplate(string templateName)
        {
            if (_templateCache.TryGetValue(templateName, out var cachedTemplate))
            {
                return cachedTemplate;
            }

            var templatePath = Path.Combine(_environment.ContentRootPath, "Templates", "EmailTemplates", templateName);
            var template = File.ReadAllText(templatePath);
            _templateCache[templateName] = template;
            return template;
        }
    }
}