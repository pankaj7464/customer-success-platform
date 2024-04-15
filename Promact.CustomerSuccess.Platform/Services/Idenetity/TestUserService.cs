using Promact.CustomerSuccess.Platform.Entities;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Promact.CustomerSuccess.Platform.Services.Idenetity
{
    public class TestUserService:IApplicationService,ITransientDependency
    {
        private readonly ApplicationUserManager _userManager;
        public TestUserService(ApplicationUserManager userManager) {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> Test()
        {
            var u = await _userManager.FindByEmailAsync("pkumar");
            return u;
        }
    }
}
