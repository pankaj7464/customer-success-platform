using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;

namespace Promact.CustomerSuccess.Platform.Services.Uttils
{
    public interface IUttilService
    {
        Task<IEnumerable<string>> GetRolesFromUrlAsync(string userId);
        Task<bool> CreateUserAsync(CreateUpdateUserDto userDto);
        Task<bool> UpdateUserAsync(CreateUpdateUserDto userDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
