using ACUnified.Business.Services;
using ACUnified.Business.Services.IServices;
using ACUnified.Data.Enum;
using ACUnified.Data.Models;
using ACUnified.Portal.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;

namespace ACUnified.Portal.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IACUEmailSender _emailService;
        private readonly IHTMLTemplateFileService _HTMLTemplate;
        private readonly IHTML2PDFService _HTML2PDFService;
        private readonly IRolesManagement _roleManagement;
        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager,
            IHTMLTemplateFileService HTMLTemplate, IHTML2PDFService HTML2PDFService, IACUEmailSender emailService
            , IRolesManagement roleManagement)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _HTMLTemplate = HTMLTemplate;
            _HTML2PDFService = HTML2PDFService;
            _emailService = emailService;
            _roleManagement = roleManagement;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            //[EmailAddress]
            //Can be email or matric number
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        // Technology stack: FormValidation



        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                //Find the account by username for student it their matric number for staff their email
                
                if (!Input.Email.Contains('@'))
                {
                    var user = await _userManager.FindByNameAsync(Input.Email);

                    if (user == null)
                    {

                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page(); // User not found
                    }

                    if (user.CurrentUserType == UserType.Student && await _userManager.CheckPasswordAsync(user, Input.Password) && user.AuthenticationStatus == AuthStatus.DefaultPW)
                    {   //update the user roles
                        await _roleManagement.GenerateRolesFromPagesAsync();
                        await _roleManagement.AddToRoles(user);
                        // Send email for password change
                        await SendPasswordChangeEmailAsync(user);
                        // return false; // Return false as user can't log in yet
                        ModelState.AddModelError(string.Empty, "Please Check Your Email for link to login");
                        return Page();
                    }
                }

                if (Input.Email.Contains('@'))
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);

                    if (user == null)
                    {

                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page(); // User not found
                    }

                    if (user.CurrentUserType == UserType.Student && await _userManager.CheckPasswordAsync(user, Input.Password) && user.AuthenticationStatus == AuthStatus.DefaultPW)
                    {   //attach a role to the student for loggin in
                        await _roleManagement.GenerateRolesFromPagesAsync();
                        await _roleManagement.AddToRoles(user);
                        // Send email for password change
                        await SendPasswordChangeEmailAsync(user);
                        // return false; // Return false as user can't log in yet
                        ModelState.AddModelError(string.Empty, "Please Check Your Email for link to login");
                        return Page();
                    }
                }


                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task SendPasswordChangeEmailAsync(ApplicationUser user)
        {
            var htmlbody = await _HTMLTemplate.GetTemplateFileAsync("Templates/MailMessage.html");
           
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var returnUrl = Url.Content("~/");
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);
            htmlbody = htmlbody.Replace("{contactInfo.firstname}", user.Firstname);
            htmlbody = htmlbody.Replace("{contactInfo.lastname}", user.Surname);
            htmlbody = htmlbody.Replace("{contactInfo.resetLink}", HtmlEncoder.Default.Encode(callbackUrl));
            //byte[] pdfbytes= _HTML2PDFService.GeneratePDF(htmlbody);
            
            await _emailService.SendEmailAsync(user.Email, "Password Change Process", htmlbody);
        }
    }
}