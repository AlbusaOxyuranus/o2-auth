using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using O2.Auth.Web.Data;
using O2.Auth.Web.Utilities;

namespace O2.Auth.Web.IoC
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddConfiguredIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AuthDbContext"));
            
            services
                .AddIdentity<O2User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    //TODO: uncomment after some tests
                    //options.Password.RequiredLength = 12; 
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IEmailSender, DummyEmailSender>();
            services.AddSingleton<IBase64QrCodeGenerator, Base64QrCodeGenerator>();
            
            return services;
        }
    }
    
    internal class DummyEmailSender : IEmailSender
    {
        private readonly ILogger<DummyEmailSender> _logger;

        public DummyEmailSender(ILogger<DummyEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogWarning("Dummy IEmailSender implementation is being used!!!");
            _logger.LogDebug($"{email}{Environment.NewLine}{subject}{Environment.NewLine}{htmlMessage}");
            return Task.CompletedTask;
        }
    }
}