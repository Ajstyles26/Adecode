using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.ComponentModel.DataAnnotations;

namespace ACUnified.Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailController> _logger;
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly string _fromEmail = "bj.adebayo@acu.edu.ng";
        private readonly string _fromName = "ACU";

        public EmailController(IConfiguration configuration, ILogger<EmailController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _host = _configuration.GetSection("Mail")["MailHost"];
            _username = _configuration.GetSection("Mail")["MailKey"];
            _password = _configuration.GetSection("Mail")["MailPassword"];

            ValidateConfiguration();
        }

        [HttpPost("welcome")]
        public async Task<IActionResult> SendWelcomeEmail([FromBody] WelcomeEmailRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var htmlMessage = GetWelcomeTemplate(request.UserName);
                await SendEmailAsync(request.Email, "Welcome to Ajayi Crowther University", htmlMessage);
                return Ok(new { message = "Welcome email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending welcome email");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody] PasswordResetRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var htmlMessage = GetPasswordResetTemplate(request.UserName, request.ResetLink);
                await SendEmailAsync(request.Email, "Password Reset Request", htmlMessage);
                return Ok(new { message = "Password reset email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending password reset email");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("notification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var htmlMessage = GetNotificationTemplate(request.Subject, request.Message);
                await SendEmailAsync(request.Email, request.Subject, htmlMessage);
                return Ok(new { message = "Notification email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification email");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        private async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_fromName, _fromEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_host, 25, false);
                await client.AuthenticateAsync(_username, _password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {email}");
                throw;
            }
        }

        private void ValidateConfiguration()
        {
            if (string.IsNullOrEmpty(_host))
                throw new ArgumentNullException(nameof(_host), "SMTP host configuration is missing");
            if (string.IsNullOrEmpty(_username))
                throw new ArgumentNullException(nameof(_username), "SMTP username configuration is missing");
            if (string.IsNullOrEmpty(_password))
                throw new ArgumentNullException(nameof(_password), "SMTP password configuration is missing");
        }


        #region Email Templates
        private string GetWelcomeTemplate(string userName)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #003366; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Welcome to ACU</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {userName},</h2>
                            <p>Welcome to Ajayi Crowther University! We're delighted to have you join our academic community.</p>
                            <p>Your account has been successfully created, and you now have access to our online portal.</p>
                            <p>If you have any questions, please don't hesitate to contact our support team.</p>
                            <br>
                            <p>Best regards,</p>
                            <p>The ACU Team</p>
                        </div>
                        <div class='footer'>
                            <p>This is an automated message, please do not reply directly to this email.</p>
                            <p>© 2025 Ajayi Crowther University. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        private string GetPasswordResetTemplate(string userName, string resetLink)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #003366; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .button {{ display: inline-block; padding: 10px 20px; background-color: #003366; 
                                  color: white; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
                        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Password Reset Request</h1>
                        </div>
                        <div class='content'>
                            <h2>Dear {userName},</h2>
                            <p>We received a request to reset your password for your ACU account.</p>
                            <p>To reset your password, please click the button below:</p>
                            <p style='text-align: center;'>
                                <a href='{resetLink}' class='button'>Reset Password</a>
                            </p>
                            <p>If you didn't request this password reset, please ignore this email or contact our support team.</p>
                            <p>This link will expire in 24 hours.</p>
                            <br>
                            <p>Best regards,</p>
                            <p>The ACU Team</p>
                        </div>
                        <div class='footer'>
                            <p>This is an automated message, please do not reply directly to this email.</p>
                            <p>© 2025 Ajayi Crowther University. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        private string GetNotificationTemplate(string subject, string message)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #003366; color: white; padding: 20px; text-align: center; }}
                        .content {{ padding: 20px; background-color: #f9f9f9; }}
                        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>{subject}</h1>
                        </div>
                        <div class='content'>
                            {message}
                            <br>
                            <p>Best regards,</p>
                            <p>The ACU Team</p>
                        </div>
                        <div class='footer'>
                            <p>This is an automated message, please do not reply directly to this email.</p>
                            <p>© 2025 Ajayi Crowther University. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }
        #endregion

        #region Request Models
        public class WelcomeEmailRequest
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string UserName { get; set; }
        }

        public class PasswordResetRequest
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string UserName { get; set; }

            [Required]
            public string ResetLink { get; set; }
        }

        public class NotificationRequest
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Subject { get; set; }

            [Required]
            public string Message { get; set; }
        }
        #endregion
    }
}