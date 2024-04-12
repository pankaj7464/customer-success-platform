using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Volo.Abp.DependencyInjection;

namespace Promact.CustomerSuccess.Platform.Services.Uttils
{
    public interface IUttilService : IScopedDependency
    {
        Task<bool> CreateUserAsync(CreateUpdateUserDto userDto);
        Task<bool> UpdateUserAsync(CreateUpdateUserDto userDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<IEnumerable<UserWithRoleDto>> GetAllUsersAsync();
        Task<UserWithRolesDto> GetUserByEmailAsync(string email);
    }
}
