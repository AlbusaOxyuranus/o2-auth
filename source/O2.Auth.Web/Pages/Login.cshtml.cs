using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using O2.Auth.Web.Data;
using O2.Auth.Web.Resources;

namespace O2.Auth.Web.Pages
{
   [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<O2User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IStringLocalizer<LoginModel> _localizer;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public LoginModel(SignInManager<O2User> signInManager, ILogger<LoginModel> logger,
            IStringLocalizer<LoginModel> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "RememberMe")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // returnUrl = returnUrl ?? Url.Content("~/");
            //
            // // Clear the existing external cookie to ensure a clean login process
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            //
            // ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Redirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWithTwoFactor", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _localizer["InvalidLoginAttempt"]);
                    return Page();
                }
            }
            
            // // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}