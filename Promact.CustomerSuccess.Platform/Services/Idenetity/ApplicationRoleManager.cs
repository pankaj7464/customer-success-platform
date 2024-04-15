using Microsoft.AspNetCore.Identity;
using Promact.CustomerSuccess.Platform.Entities;

namespace Promact.CustomerSuccess.Platform.Services.Idenetity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>, IApplicationRoleManager
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole> store, 
            IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            ILogger<RoleManager<ApplicationRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
