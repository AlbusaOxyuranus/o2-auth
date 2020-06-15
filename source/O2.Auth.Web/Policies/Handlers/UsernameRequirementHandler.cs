using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using O2.Auth.Web.Policies.Requirements;

namespace O2.Auth.Web.Policies.Handlers
{
    public class UsernameRequirementHandler : AuthorizationHandler<UsernameRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UsernameRequirement requirement)
        {
            if (Regex.IsMatch(context.User.Identity.Name, requirement.UsernamePattern))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
