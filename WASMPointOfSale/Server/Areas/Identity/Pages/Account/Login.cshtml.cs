// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WASMPointOfSale.Server.Models;

namespace WASMPointOfSale.Server.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly string _administratorEmail;
        private readonly string _administratorRoleName;

        public LoginModel(SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _administratorEmail = "administrator@trystanwilcock.com";
            _administratorRoleName = "Administrator";
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
            [EmailAddress]
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

            await SeedAdministratorUserAndRole();
        }

        private async Task SeedAdministratorUserAndRole()
        {
            ApplicationUser administratorUser = await _signInManager.UserManager.FindByEmailAsync(_administratorEmail);

            if (administratorUser == null)
            {
                administratorUser = new ApplicationUser
                {
                    Email = _administratorEmail,
                    NormalizedEmail = _administratorEmail.ToUpper(),
                    UserName = _administratorEmail,
                    EmailConfirmed = true
                };

                _logger.LogInformation("Creating administrator user.");
                await _signInManager.UserManager.CreateAsync(administratorUser, "Password1!");
            }
            else
            {
                _logger.LogInformation("Administrator user exists.");
            }

            bool administratorRoleExists = await _roleManager.RoleExistsAsync(_administratorRoleName);
            if (!administratorRoleExists)
            {
                IdentityRole newAdministratorRole = new IdentityRole
                {
                    Name = _administratorRoleName,
                    NormalizedName = _administratorRoleName.ToUpper()
                };

                _logger.LogInformation("Creating administrator role.");
                await _roleManager.CreateAsync(newAdministratorRole);
            }
            else
            {
                _logger.LogInformation("Administrator role exists.");
            }

            bool administratorUserIsInAdministratorRole = await _signInManager.UserManager.IsInRoleAsync(administratorUser, _administratorRoleName);

            if (!administratorUserIsInAdministratorRole)
            {
                _logger.LogInformation("Adding administrator user to administrator role.");
                await _signInManager.UserManager.AddToRoleAsync(administratorUser, _administratorRoleName);
            }
            else
            {
                _logger.LogInformation("Administrator user is already in administrator role.");
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
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
    }
}