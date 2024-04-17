using Auth0.ManagementApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.CustomerSuccess.Platform.Constants;
using Promact.CustomerSuccess.Platform.Services.Auth0;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Promact.CustomerSuccess.Platform.Services.Uttils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;


namespace Promact.CustomerSuccess.Platform.Services.Users
{
    public class UserService : IUserService,ITransientDependency
    {

        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IAuth0Service _auth0Service;
        private readonly IUttilService _uttilService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly string password;
        public UserService(
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IConfiguration configuration,
            IEmailService emailService,
            IUttilService uttilService,
            IAuth0Service auth0Service,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _auth0Service = auth0Service;
            _uttilService = uttilService;
            _emailService = emailService;
            _mapper = mapper;
            password = _configuration["DefaultUserPassword"];
        }

        public async Task<LoginResponse> LoginWithAuth0Token(string token)
        {
            // Retrieve user details from Auth0
            User user = await _auth0Service.GetUserDetailFromAuth0(token);
            if (user == null)
            {
                // If user details are not retrieved, return an error response
                return new LoginResponse
                {
                    Message = "User details not found in Auth0.",
                    Success = false
                };
            }

            // Find the user in your system by email
            var userDetail = await _userManager.FindByEmailAsync(user.Email);
            if (userDetail == null)
            {
                // If the user does not exist in your system, return an error response
                return new LoginResponse
                {
                    Message = "User not found in the system.",
                    Success = false
                };
            }

            // Retrieve roles for the user
            var roles = await _userManager.GetRolesAsync(userDetail);

            // Create a DTO with user details and roles
            var userWithRole = new UserWithRolesDto
            {
                UserId = userDetail.Id,
                UserName = userDetail.UserName,
                Email = userDetail.Email,
                Roles = roles.ToList() 
            };

            // Generate a JWT token for the user
            var jwtToken = await _uttilService.GenerateJwtToken(userWithRole);

            // Return a successful login response
            return new LoginResponse
            {
                Message = "Login Success",
                User = userWithRole, // Assuming you want to return the user with roles
                token = jwtToken,
                Success = true
            };
        }

        [Authorize(Policy = PolicyName.UserCreatePolicy)]
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



            var username = input.Email.Split("@")[0];
            var user = new Volo.Abp.Identity.IdentityUser(Guid.NewGuid(), username, input.Email)
            {
                Name = input.Name,
            };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Check if any roles are selected
                if (input.Roles != null && input.Roles.Any())
                {
                    foreach (var roleName in input.Roles)
                    {
                        // Check if the role exists
                        var roleExists = await _roleManager.RoleExistsAsync(roleName);
                        if (!roleExists)
                        {
                            // Role does not exist, create it
                            var newRole = new Volo.Abp.Identity.IdentityRole(Guid.NewGuid(), roleName);
                            var createRoleResult = await _roleManager.CreateAsync(newRole);
                            if (!createRoleResult.Succeeded)
                            {
                                // Failed to create role
                                throw new Exception($"Failed to create role '{roleName}'.");
                            }
                        }

                        // Add user to the role
                        var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                        if (!addToRoleResult.Succeeded)
                        {
                            // Failed to add user to the role
                            throw new Exception($"Failed to add user to the role '{roleName}'.");
                        }
                    }
                }

                // Send confirmation email
                var userConfirmationEmail = new EmailDto
                {
                    To = input.Email,
                    Subject = "Welcome to our platform!",
                    Body = Template.GenerateConfirmationEmail(input.Name, input.Email, _configuration["App:SelfUrl"])
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

        [Authorize(Policy = PolicyName.UserUpdatePolicy)]
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


        [Authorize(Policy = PolicyName.UserDeletePolicy)]

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

        [Authorize(Policy = PolicyName.RoleGetPolicy)]
        public async Task<Response> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetListAsync();

            // Select necessary properties and map to DTOs
            var rolesDto = roles.Select(role => new
            {
                Id = role.Id,
                Name = role.Name,
                isDefault = role.IsDefault
            }).ToList();

            // Return the response with the selected properties
            return new Response
            {
                message = "Roles fetched successfully",
                data = rolesDto,
                IsSuccess = true
            };
        }


        [Authorize(Policy = PolicyName.UserGetPolicy)]
        public async Task<Response> GetAllUsersWithRolesAsync()
        {
            var users = await _userRepository.GetListAsync();
            var userRoles = new List<UserWithRoleDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserWithRoleDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });
            }
            return new Response { IsSuccess = true, message = "Fetched successfully", data = userRoles };
        }

        [Authorize(Policy = PolicyName.UserGetPolicy)]
        public async Task<Response> GetAllUsersAsync()
        {
            var users = await _userRepository.GetListAsync();

            var u = _mapper.Map<List<IdentityUser>, List<UserDto>>(users);
            return new Response { IsSuccess = true, message = "Fetched successfully", data = u };
        }

        [Authorize(Policy = PolicyName.RoleCreatePolicy)]
        public async ValueTask<Response> CreateRoleAsync(string roleName)
        {
            var role = new Volo.Abp.Identity.IdentityRole(Guid.NewGuid(), roleName);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                // Handle errors
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Description}");
                }

                return new Response { message = "Error while creating role" };
            }
            else
            {
                return new Response { message = "Role created successfully", IsSuccess = true };
            }
        }


        public async Task<Response> GetUsersByRoleAsync(string roleName)
        {
            // Fetch all users in the specified role
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);


            // Select specific properties from each user
            var usersWithProperties = usersInRole.Select(user => new
            {
                user.Id,
                user.UserName,
                user.Name,
                user.Email,
            });

            // Return the response with the selected properties

            return new Response
            {
                message = "Users fetched successfully",
                data = usersWithProperties,
                IsSuccess = true
            };

        }

        [Authorize(Policy = PolicyName.AssignRolePolicy)]
        public async Task<Response> UpdateRolesAsync(UserRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new Response { message = "User not found." };
            }

            // Get the user's current roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Remove the user from all current roles
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                return new Response { message = "Failed to remove user from existing roles." };
            }

            // Iterate over the list of role IDs and add the user to each role
            foreach (var roleId in model.RoleIds)
            {
                var roleExists = await _roleManager.FindByIdAsync(roleId);
                if (roleExists == null)
                {
                    // If the role doesn't exist, skip to the next role
                    continue;
                }

                // Add the user to the role
                var addResult = await _userManager.AddToRoleAsync(user, roleExists.Name);
                if (!addResult.Succeeded)
                {
                    // If adding the user to the role fails, return an error response
                    return new Response { message = $"Failed to assign role: {roleExists.Name}" };
                }
            }
            // Return success response if all roles were assigned successfully
            return new Response { message = "Roles updated successfully." };
        }

        [Authorize(Policy = PolicyName.RoleDeletePolicy)]
        public async Task<Response> DeleteRoleAsync([FromRoute] string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                // Handle error
                // You can check result.Errors for details on why the deletion failed
                throw new Exception("Failed to delete role.");
            }
            return new Response { message = "Deleted successfully." };
        }
    }
   public class LoginResponse
    {
        public string token { get; set; }
        public object User { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class UserRoleDto
    {
        public string UserId { get; set; }
        public IEnumerable<string> RoleIds { get; set; }
    }

    public class Response
    {
        public string message { get; set; }
        public object data { get; set; }
        public bool? IsSuccess { get; set; }
    }
}
