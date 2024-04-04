
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Promact.CustomerSuccess.Platform.Services.Uttils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using static Volo.Abp.UI.Navigation.DefaultMenuNames.Application;

namespace Promact.CustomerSuccess.Platform.Services.Users
{

    public class UserService :  CrudAppService<User, UserDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserDto, CreateUpdateUserDto>, IUserService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        IEmailService _emailService;
        IUttilService _uttilService;

        public UserService(
            IRepository<User, Guid> userRepository,
            IUttilService uttilService,
            IConfiguration configuration,
            IEmailService emailService,
            IMapper mapper):base(userRepository)

        {
            _uttilService = uttilService;
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
            _mapper = mapper;
        }





        public override async Task<UserDto> CreateAsync(CreateUpdateUserDto input)
        {
            try
            {
                // Attempt to create the user
                var isCreated = await _uttilService.CreateUserAsync(input);
                if (isCreated)
                {
                    // If user creation is successful, call the base method to create the user
                    var createdUser = await base.CreateAsync(input);
                    if (createdUser != null)
                    {
                        // Prepare and send the user confirmation email
                        var userConfirmationEmail = new EmailDto
                        {
                            To = input.Email,
                            Subject = "Welcome to our platform!",
                            Body = Template.GenerateConfirmationEmail(input.UserName, input.Email, _configuration["App:SelfUrl"])
                        };
                        await _emailService.SendEmail(userConfirmationEmail);

                        // Prepare and send the admin notification email
                        // Assuming admin.Email is correctly defined or retrieved
                        //var adminNotificationEmail = new EmailDto
                        //{
                        //    To = admin.Email, // Ensure admin.Email is correctly defined
                        //    Subject = "New User registered",
                        //    Body = $"A new user has registered with the email: {input.Email}. Please verify them if valid."
                        //};
                        //await _emailService.SendEmail(adminNotificationEmail);

                        // Return the created user
                        return createdUser;
                    }
                    else
                    {
                        // Handle the case where the base method fails to create the user
                        // This could involve logging the failure or throwing an exception
                        return null;
                    }
                }
                else
                {
                    // Handle the case where user creation fails
                    // This could involve logging the failure or throwing an exception
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as appropriate for your application
                // This could involve logging the exception or rethrowing it
                throw; // Rethrow the exception or handle it differently
            }
        }

        public override async Task<PagedResultDto<UserDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var userDtos = new List<UserDto>();

           var users =await _uttilService.GetAllUsersAsync();

            return new PagedResultDto<UserDto>(userDtos.Count, userDtos);
        }

    }


    public class Response
    {
        public string message { get; set; }
        public UserDto User { get; set; }
        public int? IsSuccess { get; set; }  = 0;
    }
}
