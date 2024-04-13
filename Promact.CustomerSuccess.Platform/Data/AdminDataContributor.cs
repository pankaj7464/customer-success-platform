using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Promact.CustomerSuccess.Platform.Constants;
using Volo.Abp.Uow;

namespace Promact.CustomerSuccess.Platform.Data
{
    public class AdminDataContributor : IDataSeedContributor, ITransientDependency
    {

        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IConfiguration _config;
        public AdminDataContributor(
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            IConfiguration config
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }
        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var existingUser = await _userManager.FindByEmailAsync(AdminCredentials.AdminEmail);
            if (existingUser != null)
            {
                // User with the same email already exists
                throw new Exception("User with this email already exists.");
            }
            var user = new IdentityUser(Guid.NewGuid(), AdminCredentials.AdminUsername, AdminCredentials.AdminEmail)
            {
                Name = AdminCredentials.AdminUsername,
            };
            var result = await _userManager.CreateAsync(user, AdminCredentials.AdminPassword);

            if (result.Succeeded)
            {
                var roleName = AdminCredentials.Role;
                // Check if any roles are selected
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    // Role does not exist, create it
                    var newRole = new Volo.Abp.Identity.IdentityRole(Guid.NewGuid(), roleName);
                    var createRoleResult = await _roleManager.CreateAsync(newRole);
                    if (!createRoleResult.Succeeded)
                    {
                        // Failed to create role
                        throw new Exception($"Failed to create role ''.");
                    }
                }

                // Add user to the role
                var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!addToRoleResult.Succeeded)
                {
                    // Failed to add user to the role
                    throw new Exception($"Failed to add user to the role '{roleName}'.");
                }

            }
            else
            {

                throw new Exception("Failed to create user.");
            }

        }
    }
}