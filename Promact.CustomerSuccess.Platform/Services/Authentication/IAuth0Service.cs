using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.Auth0
{
    public interface IAuth0Service : IApplicationService
    {
        /// <summary>
        /// Exchanges an Auth0 token for a JWT token.
        /// </summary>
        /// <param name="token">The Auth0 token to exchange.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a token response.</returns>
        Task<TokenResponse> ExchangeToken(string token);
    }
}
