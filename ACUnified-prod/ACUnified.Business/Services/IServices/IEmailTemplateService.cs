using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACUnified.Portal.Data;
using static QRCoder.PayloadGenerator;

namespace ACUnified.Business.Services.IServices
{
 public interface IEmailTemplateService
    {
        string GetWelcomeTemplate(string userName);
        string GetPasswordResetTemplate(string resetLink, string userName);
        string GetNotificationTemplate(string subject, string message);
    }
}