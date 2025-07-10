// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ACUnified.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ACUnified.Portal.Areas.Identity.Pages.Account
{
    public class LogoutModel2 : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel2(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("User logged out.");
             return Redirect("~/Identity/Account/Logout");  
        }
    }
}
