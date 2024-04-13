
using Microsoft.IdentityModel.Tokens;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Uttils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;


namespace Promact.CustomerSuccess.Platform.Services.Auth0
{
    public class Auth0Service : IAuth0Service, IScopedDependency
    {
        private readonly IConfiguration _configuration;
        private readonly IUttilService _uttilService;
        public Auth0Service(IConfiguration configuration, HttpClient httpClient,
        ICurrentUser currentUser, IUttilService uttilService)
        {
            _uttilService = uttilService;
            _configuration = configuration;
        }

        public async Task<TokenResponse> ExchangeToken(string token)
        {
            // Decode the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Extract user details and roles
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (email != null)
            {
                var user = await _uttilService.GetUserByEmailAsync(email);
                if (user != null)
                {
                    var jwt_token = GenerateJwtToken(user);
                    return new TokenResponse { token = jwt_token, Data = user };
                }
                else new TokenResponse { Message = "You are not registered user", };
            }

            return new TokenResponse { Message = "Not valid token", };

        }

        private string GenerateJwtToken(UserWithRolesDto user)
        {
            var securityKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.NameIdentifier, user.UserId),
                 new Claim(ClaimTypes.Name, user.UserName),
                 new Claim(ClaimTypes.Email, user.Email)
             };


            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

    }
    public class TokenResponse
    {
        public string token { get; set; }
        public string Message { get; set; }
        public object Data { get; internal set; }
    }
}
