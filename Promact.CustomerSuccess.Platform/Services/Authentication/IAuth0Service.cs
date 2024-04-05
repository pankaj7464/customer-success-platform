using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.Auth0
{
    public interface IAuth0Service : IApplicationService
    {
        Task<TokenResponse> ExchangeToken(string token);
    }
}
