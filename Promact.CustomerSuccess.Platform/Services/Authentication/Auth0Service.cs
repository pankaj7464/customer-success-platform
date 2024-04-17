
using Auth0.ManagementApi.Models;
using Newtonsoft.Json;
using Promact.CustomerSuccess.Platform.Services.Uttils;
using System.Net.Http.Headers;
using Volo.Abp.DependencyInjection;


namespace Promact.CustomerSuccess.Platform.Services.Auth0
{
    public class Auth0Service : IAuth0Service, IScopedDependency
    {
        private readonly IConfiguration _configuration;
        private readonly IUttilService _uttilService;
        private readonly HttpClient _httpClient;
        public Auth0Service(IConfiguration configuration, HttpClient httpClient,
        IUttilService uttilService)
        {
            _uttilService = uttilService;
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<User> GetUserDetailFromAuth0(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_configuration["Auth0:Domain"] + "/userinfo");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(jsonResult);
                    return user; // Return the user detail as a successful response
                }
                else
                {
                    // Handle unsuccessful response
                    return null;
                }
            }
            catch (Exception ex)
            {
              
                return null;
            }
        }
    }

}
