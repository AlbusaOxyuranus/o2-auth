using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using O2.Auth.Web.Data;
using IdentityServer4.EntityFramework.DbContexts;

namespace O2.Auth.Web.StartupHelper
{
    internal static class DatabaseExtensions
    {
        internal static async Task EnsureDbUpToDateAsync(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                await authDbContext.Database.MigrateAsync();

                var grantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                await grantDbContext.Database.MigrateAsync();
            }
        }
    }
}