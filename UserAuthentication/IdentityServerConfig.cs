using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuthentication
{
    // todi: move to config http://docs.identityserver.io/en/latest/quickstarts/1_client_credentials.html
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo", new string[] { "rc.api.garndma" }),
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
                new Client
                {
                    ClientId = "some_client_id",
                    ClientSecrets = {
                        new Secret("some_secret".ToSha256())
                    },
                    AllowedGrantTypes = new List<string>() { GrantType.ResourceOwnerPassword, GrantType.ClientCredentials },
                    AllowedScopes =
                    {
                        "ApiOne"
                    }
                }
            };
    }
}
