using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Identity.Api.Configuration
{
    public class ISConfiguration
    {
        private readonly IConfiguration _configuration;

        public ISConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<Client> Clients
        {
            get {
                string SPAUrl = _configuration.GetSection("Uri").GetValue<string>("SPA");
                return new List<Client>
                {
                    new Client
                    {
                        ClientId = "spa",
                        ClientName = "SPA Client",
                        AllowedGrantTypes = GrantTypes.Code,
                        RequireClientSecret = false,

                        RedirectUris = {$"{SPAUrl}/callback.html" },
                        PostLogoutRedirectUris = { $"{SPAUrl}/index.html" },
                        AllowedCorsOrigins = { SPAUrl },

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            ISScopes.CourseAPI,
                            "API"
                        },

                        AlwaysIncludeUserClaimsInIdToken = true
                    },
                    new Client
                    {
                        ClientId = "swagger_api",
                        ClientName = "Swagger UI",
                        ClientSecrets = {new Secret("swagger_secret".Sha256())},

                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        RequireClientSecret = false,

                        RedirectUris = {"https://localhost:7001/swagger/oauth2-redirect.html"},
                        AllowedCorsOrigins = {"https://localhost:7001"},
                        AllowedScopes = {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            ISScopes.CourseAPI
                        },
                        AlwaysIncludeUserClaimsInIdToken = true
                    },
                    new Client
                    {
                        ClientId = "client_id",
                        ClientSecrets = { new Secret("client_secret".ToSha256())},

                        AllowedGrantTypes = GrantTypes.ClientCredentials,

                        AllowedScopes =
                        { 
                            ISScopes.CourseAPI
                        }
                    }
                };
            }
        }

        public IEnumerable<ApiScope> Scopes =>
            new List<ApiScope>
            {
                new ApiScope(ISScopes.CourseAPI),
                new ApiScope("API")
            };

        public IEnumerable<ApiResource> Resourses =>
            new List<ApiResource>
            {
                new ApiResource(ISScopes.CourseAPI) {Scopes = {ISScopes.CourseAPI } }
            };
    }

    static class ISScopes
    {
        public const string CourseAPI = "CourseAPI";
    }
}
