using Auth0.ManagementApi.Models;
using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.Auth0
{
    public interface IAuth0Service 
    {
        public Task<User> GetUserDetailFromAuth0(string token);
    }
}
