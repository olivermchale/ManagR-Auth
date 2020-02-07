using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UserAuthentication.Models.ViewModels;

namespace UserAuthentication
{
    public class IDAuthService : IIDAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public IDAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Authenticate(LoginVm loginInfo)
        {
            var client = _httpClientFactory.CreateClient();

            var discovery = await client.GetDiscoveryDocumentAsync("https://localhost:5001/");

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discovery.TokenEndpoint,

                ClientId = "some_client_id",
                ClientSecret = "some_secret",
                Scope = "ApiOne",
                UserName = loginInfo.Username,
                Password = loginInfo.Password
            });

            return tokenResponse.AccessToken;
        }
    }
}
