using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Api.Configuration
{
    public class ISConfiguration
    {
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256())},

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes =
                    {
                        "CoursesAPI"
                    }
                }
            };

        public static IEnumerable<ApiResource> GetApiResourses() =>
            new List<ApiResource>
            {
                new ApiResource("CourseAPI")
            };

        public static IEnumerable<IdentityResource> GetIdentityResourses() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId()
            };
    }
}
