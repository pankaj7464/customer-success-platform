using Auth0.ManagementApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Uttils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Application.Services;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;


namespace Promact.CustomerSuccess.Platform.Services.Auth0
{
    public class Auth0Service : ApplicationService, IAuth0Service
    {
        private readonly IManagementConnection _managementConnection;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ICurrentUser _currentUser;
        private readonly IUttilService _UttilService;
        public Auth0Service(IConfiguration configuration, HttpClient httpClient, ICurrentUser currentUser , IUttilService uttilService)
        {
            _httpClient = httpClient;
            _currentUser = currentUser;
            _UttilService = uttilService;
            _configuration = configuration;
        }

        public async Task<string> ExchangeToken(string token)
        {
            // Decode the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Extract user details and roles
            var UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (UserId != null)
            {
                var role =await _UttilService.GetRolesFromUrlAsync(UserId);
                var claim = CreateClaims(UserId, role);
                var jwt_token = await GenerateJwtTokenAsync(claim);
                return jwt_token;
            }

            return "something went wrong";

        }

        /// <summary>
        /// Generates a JWT token for the specified user with the provided claims.
        /// </summary>
        /// <returns>
        /// Returns JWT token as a string.
        /// </returns>
        private async Task<string> GenerateJwtTokenAsync(IEnumerable<Claim> claims)
        {

            // Create symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"]));

            // Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1), // Token expiry time
                SigningCredentials = creds
            };

            // Create token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Generate token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return token as string
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Creates claims for a user, including user ID, email, username, email verification status, and roles.
        /// </summary>
        /// <returns>
        /// Returns a collection of claims containing user information and roles.
        /// </returns>
        private static IEnumerable<Claim> CreateClaims(string userId, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(AbpClaimTypes.UserId, userId),

            };


            foreach (var role in roles)
            {
                claims.Add(new Claim(AbpClaimTypes.Role, role));
            }

            return claims;
        }

    }
}
