using Newtonsoft.Json;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using System.Text;
using Volo.Abp.Identity;
namespace Promact.CustomerSuccess.Platform.Services.Uttils
{
    public class UttillService : IUttilService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IdentityUserManager _userManager;

        public UttillService(IConfiguration configuration, HttpClient httpClient, IdentityUserManager userManager)
        {
            _userManager = userManager;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configuration["Auth0:Access_token"]);
        }
        public async Task<UserWithRolesDto> GetUserByEmailAsync(string email)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                // User with the provided email does not exist
                throw new Exception("User not found.");
            }

            // Retrieve roles associated with the user
            var roles = await _userManager.GetRolesAsync(user);

            // Map user and roles to DTO
            var userWithRolesDto = new UserWithRolesDto
            {
                UserId = user.Id.ToString(),
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList() // Convert roles to a list
            };

            return userWithRolesDto;
        }


        public async Task<bool> CreateUserAsync(CreateUpdateUserDto userDto)
        {
            try
            {
                // Prepare user data object
                var userData = new
                {
                    name = userDto.Name,
                    email = userDto.Email,
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
