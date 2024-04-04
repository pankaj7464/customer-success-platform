using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace Promact.CustomerSuccess.Platform.Services.Auth0
{
    public interface IAuth0Service:IApplicationService
    {
        Task<string> ExchangeToken(string token);
    }
}
