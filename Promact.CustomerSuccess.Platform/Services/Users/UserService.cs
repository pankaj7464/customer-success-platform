
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.Users
{

    public class UserService :  CrudAppService<User, UserDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserDto, CreateUpdateUserDto>, IUserService
    {
        private readonly IRepository<Entities.User, Guid> _userRepository;
        private readonly IRepository<UserRole, Guid> _userRoleRepository;
        private readonly IRepository<Entities.Role, Guid> _roleRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        IEmailService _emailService;

        public UserService(
            IRepository<User, Guid> userRepository,
            IRepository<UserRole, Guid> userRoleRepository,
            IRepository<Role, Guid> roleRepository,
            IConfiguration configuration,
            IEmailService emailService,
            IMapper mapper):base(userRepository)

        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _configuration = configuration;
            _emailService = emailService;

            _mapper = mapper;
        }

        public override async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            // Call base method to create the user
            var createdUser = await base.CreateAsync(input);
            var role = await _roleRepository.FirstOrDefaultAsync(ad => ad.Name == "Admin");
            var client = await _roleRepository.FirstOrDefaultAsync(ad => ad.Name == "Client");
            if(createdUser != null && client!=null )
            {
                // Create new UserRole
                var newUserRole = new UserRole
                {
                    UserId = client.Id,
                    RoleId = createdUser.Id
                };
                await _userRoleRepository.InsertAsync(newUserRole, autoSave: true);
            }
            
            if (role != null)
            {
                var adminRole = await _userRoleRepository.FirstOrDefaultAsync(ar => ar.RoleId == role.Id);
                if (adminRole != null)
                {
                    var admin = await _userRepository.FirstOrDefaultAsync(user => user.Id == adminRole.UserId);
                    if(admin != null)
                    {
                        var userConfirmationEmail = new EmailDto
                        {
                            To = input.Email,
                            Subject = "Welcome to our platform!",
                            Body = Template.GenerateConfirmationEmail(input.UserName, input.Email, _configuration["App:SelfUrl"])
                        };
                        var adminNotificationEmail = new EmailDto
                        {
                            To = admin.Email, 
                            Subject = "New User registered",
                            Body = $"A new user has registered with the email: {input.Email}. Please verify them if valid."
                        };
                        _emailService.SendEmail(userConfirmationEmail);
                        _emailService.SendEmail(adminNotificationEmail);
                    }
                }
            }


            // Return the created user
            return createdUser;
        }

        public async Task<Response> GetUserByUsernameAndEmailAsync(string username, string email)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.UserName == username && u.Email == email );

            if (user == null)
            {
                // User does not exist
                return new Response { message = "User not exist", User = null };
            }
            if (!user.active)
            {
               
                return new Response { message = "You are not verified user", User = null,IsSuccess=2 };
            }

            var userDto = _mapper.Map<User, UserDto>(user);

            // Fetch the UserRole
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id);

            if (userRole != null)
            {
                // Fetch the associated Role
                var role = await _roleRepository.GetAsync(userRole.RoleId);

                // Map RoleDto
                userDto.Role = _mapper.Map<Role, RoleDto>(role);
            }
           
            // User exists
            return new Response { message = "You have successfully login", User = userDto ,IsSuccess=1};
        }

        public async Task<UserDto> GetDetailByEmailAsync(string email)
        {
            var user = (await _userRepository.GetListAsync(u => u.Email == email)).FirstOrDefault();
            return ObjectMapper.Map<User, UserDto>(user);
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                // Handle if roleName is null or empty
                return null;
            }

            // Fetch the role
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());

            if (role == null)
            {
                // Handle if role not found
                return null;
            }

            // Fetch users with the specified role
            var userRoles = await _userRoleRepository.GetListAsync(ur => ur.RoleId == role.Id);
            var userIds = userRoles.Select(ur => ur.UserId);

            // Fetch users
            var users = await _userRepository.GetListAsync(u => userIds.Contains(u.Id));

            // Map users to UserDto
            var userDtos = users.Select(user => _mapper.Map<User, UserDto>(user)).ToList();

            return userDtos;
        }

        public override async Task<PagedResultDto<UserDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var users = await base.Repository.ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var userDto = _mapper.Map<User, UserDto>(user);

                // Fetch the UserRole
                var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id);

                if (userRole != null)
                {
                    // Fetch the associated Role
                    var role = await _roleRepository.GetAsync(userRole.RoleId);

                    // Map RoleDto
                    userDto.Role = _mapper.Map<Role, RoleDto>(role);
                }

                userDtos.Add(userDto);
            }

            return new PagedResultDto<UserDto>(userDtos.Count, userDtos);
        }

        public async Task<bool> ToggleUserAccountStatus(Guid userId, bool isActive)
        {
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                user.active = isActive;
                await _userRepository.UpdateAsync(user, true);

                var notificationEmail = new EmailDto
                {
                    To = user.Email,
                    Subject = isActive ? "Your account has been activated" : "Your account has been deactivated",
                    Body = isActive ? "Your account has been activated. You can now access our platform." : "Your account has been deactivated. If you believe this is an error, please contact support."
                };

                _emailService.SendEmail(notificationEmail); 
                return true; // Account status updated successfully
            }

            return false; // User not found
        }

        //To be implemented
        private string GenerateJwtToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }


    public class Response
    {
        public string message { get; set; }
        public UserDto User { get; set; }
        public int? IsSuccess { get; set; }  = 0;
    }
}
