using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace ACUnified.Business.Services.IServices
{

    public interface IACUEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message);
        public Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] bytes);

    }

}