
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.CustomerSuccess.Platform.Constants;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Services.Users
{

    public class UserService : IUserService, IScopedDependency
    {

        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly string password;

        public UserService(
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            IConfiguration configuration,
                IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
            IEmailService emailService,

            IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _mapper = mapper;
            password = _configuration["DefaultUserPassword"];

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
            var userRoles = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserDto
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
        public async Task<Response> AssignRole(UserRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return new Response { message = "User not found." };
            }

            var roleExists = await _roleManager.FindByIdAsync(model.RoleId);
            if (roleExists == null)
            {
                return new Response { message = "Role does not exist." };
            }

            // Check if the user is already in the role
            var isInRole = await _userManager.IsInRoleAsync(user, roleExists.Name);

            if (isInRole)
            {
                // If the user is already in the role, remove them first
                var removeResult = await _userManager.RemoveFromRoleAsync(user, roleExists.Name);
                if (!removeResult.Succeeded)
                {
                    return new Response { message = "Failed to remove existing role." };
                }
            }

            // Add the user to the role
            var addResult = await _userManager.AddToRoleAsync(user, roleExists.Name);
            if (!addResult.Succeeded)
            {
                return new Response { message = "Failed to assign role." };
            }

            return new Response { message = "Role assigned successfully." };
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

    public class UserRoleDto
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }

    public class Response
    {
        public string message { get; set; }
        public object data { get; set; }
        public bool? IsSuccess { get; set; }
    }
}
