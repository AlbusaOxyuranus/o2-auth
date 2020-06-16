using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using O2.Auth.Web.Data;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Secret = IdentityServer4.EntityFramework.Entities.Secret;


namespace O2.Auth.Web.IoC
{
     public static class IdentityServerExtensions
    {
        public static IServiceCollection AddConfiguredIdentityServer(this IServiceCollection services,
            IHostingEnvironment environment, IConfiguration configuration)
        {
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                // using in memory, but we could also get it, for example, from the database

                // access to data regarding the user's identity
                .AddInMemoryIdentityResources(GetIdentityResources())
                // APIs that may be accessed
                .AddInMemoryApiResources(GetApis())
                // client applications that may access users data and APIs on the user's behalf
                .AddInMemoryClients(GetClients())
                // configures IdentityServer integration with ASP.NET Core Identity
                .AddAspNetIdentity<O2User>()
                
                // more about EF integration:
                // - http://docs.identityserver.io/en/latest/quickstarts/7_entity_framework.html
                // - http://docs.identityserver.io/en/latest/reference/ef.html?highlight=dbcontext

                // is we wanted the configurations in the database, we would need this
                //.AddConfigurationStore()
                // use the database to store operational data, so it's persisted across servers in a cluster and/or restarts
                // otherwise, as an example, refresh tokens become invalid
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(configuration.GetConnectionString("PersistedGrantDbContext"),
                            npgOptions =>
                                npgOptions.MigrationsAssembly(typeof(IdentityServerExtensions).Assembly.GetName()
                                    .Name));
                })
                // to avoid bombarding the db with checks, make use of cache
                .AddInMemoryCaching();
                // we could implement our own cache if we wanted, for instance, using Redis
                //.AddResourceStoreCache<SomeCacheImplementation>()


            if (environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            return services;
        }

        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var profile = new IdentityResources.Profile();
            profile.Required = true;
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                profile
            };
        }

        private static IEnumerable<ApiResource> GetApis()
        {
            var apiResource = new ApiResource("CertificateManagement", "Certificate Management");
            apiResource.Scopes.First().Required = true;
            return new[]
            {
                apiResource
            };
        }

        private static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "WebFrontend",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new IdentityServer4.Models.Secret("secret".Sha256())
                    },
                    RedirectUris = new[] {"http://localhost:5000/signin-oidc"},
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "CertificateManagement",
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 60,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    //RequireConsent = false
                }
            };
        }
    }
}