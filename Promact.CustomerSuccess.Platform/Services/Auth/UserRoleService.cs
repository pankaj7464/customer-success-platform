using Microsoft.AspNetCore.Mvc;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.Auth
{
    public class UserRoleService : CrudAppService<UserRole, UserRoleDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserRole, CreateUpdateUserRole>
    {
        private readonly IRepository<UserRole, Guid> _repository;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IRepository<Role, Guid> _roleRepository;
        private readonly IEmailService _emailService;
        public UserRoleService(IRepository<UserRole, Guid> repository,IRepository<User,Guid> userRepository, IRepository<Role, Guid> roleRepository) : base(repository)
        {

            this._repository = repository;
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
        }

        [HttpPost("assign-role")]
        public async Task<UserRoleDto> CreateOrUpdateUserRoleAsync(Guid userId, Guid roleId)
        {
            // Check if UserRole already exists for the given userId and roleId
            var existingUserRole = await _repository.FirstOrDefaultAsync(ur => ur.UserId == userId);

            if (existingUserRole != null)
            {
                // Update existing UserRole
                existingUserRole.UserId = userId;
                existingUserRole.RoleId = roleId;
                await Repository.UpdateAsync(existingUserRole);

                return ObjectMapper.Map<UserRole, UserRoleDto>(existingUserRole);
            }
            else
            {
                // Create new UserRole
                var newUserRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };
                await Repository.InsertAsync(newUserRole, autoSave: true);

                var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == userId);
                var role = await _roleRepository.FirstOrDefaultAsync(r=>r.Id == userId);

                if (user != null && role!=null)

                {
                    var email = new EmailDto
                    {
                        To = user.Email,
                        Subject =  "Role Updated",
                        Body = $@"You are now {role.Name}"
                    };
                    _emailService.SendEmail(email);
                }

                return ObjectMapper.Map<UserRole, UserRoleDto>(newUserRole);
            }
        }

    }
}
