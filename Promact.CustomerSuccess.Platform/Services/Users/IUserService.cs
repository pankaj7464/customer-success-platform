using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.Users
{
    public interface IUserService:IApplicationService
    {
        Task<UserDto> GetDetailByEmailAsync(string email);
    }
}
