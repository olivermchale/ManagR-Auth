using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration;
        public IDAuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(LoginVm loginInfo)
        {
            var client = _httpClientFactory.CreateClient();

            var url = _configuration.GetValue<string>("IDConfigUrl");
            var discovery = await client.GetDiscoveryDocumentAsync(url);

            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discovery.TokenEndpoint,

                ClientId = "some_client_id",
                ClientSecret = "some_secret",
                Scope = "ManagR",
                UserName = loginInfo.Username,
                Password = loginInfo.Password
                
            });

            return tokenResponse.AccessToken;
        }
    }
}
