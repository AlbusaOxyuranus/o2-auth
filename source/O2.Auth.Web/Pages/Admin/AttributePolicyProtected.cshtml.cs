using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace O2.Auth.Web.Pages.Admin
{
    [Authorize(Policy = "SamplePolicy")]
    public class AttributePolicyProtectedModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}