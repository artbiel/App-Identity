using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity.Api.Configuration
{
    public class ISConfiguration
    {
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,

                    RedirectUris = {"https://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = {"https://localhost:5003/index.html" },
                    AllowedCorsOrigins = {"https://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "CoursesAPI"
                    }
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

        public static IEnumerable<ApiScope> Scopes =>
            new List<ApiScope>
            {
                new ApiScope(ISScopes.CourseAPI)
            };

        //public static IEnumerable<IdentityResource> GetIdentityResourses() =>
        //    new List<IdentityResource>
        //    {
        //        new IdentityResources.OpenId()
        //    };
    }

    static class ISScopes
    {
        public const string CourseAPI = "CourseAPI";
    }
}
