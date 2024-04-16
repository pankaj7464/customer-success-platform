using Microsoft.IdentityModel.Tokens;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Identity;
namespace Promact.CustomerSuccess.Platform.Services.Uttils
{
    public class UttillService : IUttilService
    {
        private readonly IConfiguration _configuration;
        private readonly IdentityUserManager _userManager;

        public UttillService(IConfiguration configuration, HttpClient httpClient, IdentityUserManager userManager)
        {
            _userManager = userManager;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
       
        }
   
        public async Task<string> GenerateJwtToken(UserWithRolesDto user)
        {
            if (user != null)
            {
                var securityKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
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
            return null;

        }

      
    }


}
