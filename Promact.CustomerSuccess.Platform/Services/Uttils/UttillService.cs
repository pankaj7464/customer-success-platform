using Newtonsoft.Json;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Users;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using static Volo.Abp.UI.Navigation.DefaultMenuNames.Application;

namespace Promact.CustomerSuccess.Platform.Services.Uttils
{
    public class UttillService : IUttilService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UttillService(IConfiguration configuration, HttpClient httpClient)
        {
            
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configuration["Auth0:Access_token"]);

        }

        public async Task<IEnumerable<string>> GetRolesFromUrlAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_configuration["Auth0:Domain"]}/api/v2/users/{userId}/roles");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<RoleDto>>(content);

                return roles.Select(r => r.Name);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return Enumerable.Empty<string>();
            }
        }

        public async Task<bool> CreateUserAsync(CreateUpdateUserDto userDto)
        {
            try
            {
                // Prepare user data object
                var userData = new
                {
                    name = userDto.Name,
                    email=userDto.Email,
                    password = _configuration["Auth0:DefaultPassword"],
                    connection = "Username-Password-Authentication"                                                
                };

                // Serialize user data to JSON
                var json = JsonConvert.SerializeObject(userData);

                // Prepare HTTP request
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_configuration["Auth0:Domain"]}/api/v2/users"),
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                // Send HTTP request
                var response = await _httpClient.SendAsync(request);

                // Check if request was successful
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {

                    // Log or handle unsuccessful response
                    Console.WriteLine($"Failed to create user. Status code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Exception occurred while creating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(CreateUpdateUserDto userDto)
        {
            try
            {
                // Implement logic to update a user in Auth0 using userDto
                // Example: Call Auth0 Management API to update user
                // var response = await _managementConnection.UpdateUserAsync(userDto);
                // return response.IsSuccessStatusCode;
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                // Implement logic to delete a user in Auth0 using userId
                // Example: Call Auth0 Management API to delete user
                // var response = await _managementConnection.DeleteUserAsync(userId);
                // return response.IsSuccessStatusCode;
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return false;
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_configuration["Auth0:Domain"]}/api/v2/users");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserDto>>(content);

                return users;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return null;
            }
        }

     
    }
}
