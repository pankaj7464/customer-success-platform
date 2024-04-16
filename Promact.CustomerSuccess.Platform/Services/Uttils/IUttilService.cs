
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Volo.Abp.DependencyInjection;

namespace Promact.CustomerSuccess.Platform.Services.Uttils
{
    public interface IUttilService : IScopedDependency
    {
        Task<string> GenerateJwtToken(UserWithRolesDto user);

    }
}
