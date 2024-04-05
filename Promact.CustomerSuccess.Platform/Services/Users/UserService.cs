
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Services.Users
{

    public class UserService : IUserService, IScopedDependency
    {
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly string password = "Welcome@123";

        public UserService(
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            IConfiguration configuration,
            IEmailService emailService,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _mapper = mapper;
        }



        public async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            // Check if a user with the same email already exists
            var existingUser = await _userManager.FindByEmailAsync(input.Email);
            if (existingUser != null)
            {
                // User with the same email already exists
                throw new Exception("User with this email already exists.");
            }

            // User does not exist, proceed with creating a new user
            var user = _mapper.Map<Volo.Abp.Identity.IdentityUser>(input);
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {

                
                var role = await _roleManager.FindByNameAsync("Client");

                if (role != null)
                {

                await _userManager.AddToRoleAsync(user, "Client");
                }
                else
                {
                    var clientRole = new Volo.Abp.Identity.IdentityRole(Guid.NewGuid(),"Client");
                    var createRoleResult = await _roleManager.CreateAsync(clientRole);
                    await _userManager.AddToRoleAsync(user,"Client");
                }

                // Send confirmation email
                var userConfirmationEmail = new EmailDto
                {
                    To = input.Email,
                    Subject = "Welcome to our platform!",
                    Body = Template.GenerateConfirmationEmail(input.UserName, input.Email, _configuration["App:SelfUrl"])
                };
                await _emailService.SendEmail(userConfirmationEmail);

                return _mapper.Map<UserDto>(user);
            }
            else
            {
                // Handle error
                // You can check result.Errors for details on why the creation failed
                throw new Exception("Failed to create user.");
            }
        }

        public async Task<UserDto> UpdateAsync(Guid id, CreateUpdateUserDto input)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            _mapper.Map(input, user);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return _mapper.Map<UserDto>(user);
            }
            else
            {
                // Handle error
                // You can check result.Errors for details on why the update failed
                throw new Exception("Failed to update user.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                // Handle error
                // You can check result.Errors for details on why the deletion failed
                throw new Exception("Failed to delete user.");
            }
        }
    }

    public class Response
    {
        public string message { get; set; }
        public UserDto User { get; set; }
        public int? IsSuccess { get; set; } = 0;
    }
}
