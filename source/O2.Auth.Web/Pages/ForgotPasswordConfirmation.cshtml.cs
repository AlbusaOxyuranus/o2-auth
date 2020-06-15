using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace O2.Auth.Web.Pages
{
    [AllowAnonymous] 
    public class ForgotPasswordConfirmationModel : PageModel 
    { 
        public void OnGet() 
        { 
        } 
    } 
} 
