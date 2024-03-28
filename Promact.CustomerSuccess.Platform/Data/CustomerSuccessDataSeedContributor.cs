using Promact.CustomerSuccess.Platform.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Data
{

    public class CustomerSuccessDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IRepository<Role, Guid> _roleRepository;
        private readonly IRepository<UserRole, Guid> _userRoleRepository;

        public CustomerSuccessDataSeedContributor(
            IRepository<User, Guid> userRepository,
            IRepository<Role, Guid> roleRepository,
            IRepository<UserRole, Guid> userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public  async Task SeedAsync(DataSeedContext context)
        {
            // Check if the admin role already exists
            var adminRole = await _roleRepository.FirstOrDefaultAsync(r => r.Name == "Admin");

            if (adminRole == null)
            {
                // Create a new admin role
                adminRole = new Role
                {
                    Name = "Admin",
                    Description = "Admin Role Have full access of the system"
                };

                // Add the admin role to the database
                await _roleRepository.InsertAsync(adminRole, autoSave: true);
            }

            // Check if the admin user already exists
            var adminUser = await _userRepository.FirstOrDefaultAsync(u => u.UserName == "admin");


            if (adminUser == null)
            {
                // Create a new admin user
                adminUser = new User
                {
                    UserName = "auth0|65f6dc09cc7dca58831d9893",
                    Name = "Admin",
                    Email = "admin1@gmail.com",
                    active = true 
                };

                // Add the admin user to the database
                await _userRepository.InsertAsync(adminUser, autoSave: true);
            }

            // Check if the relationship between admin user and admin role already exists
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur =>
                ur.UserId == adminUser.Id && ur.RoleId == adminRole.Id);

            if (userRole == null)
            {
                // Establish the relationship between admin user and admin role
                userRole = new UserRole
                {
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id
                };

                // Add the relationship to the database
                await _userRoleRepository.InsertAsync(userRole, autoSave: true);
            }
        }
    }
}
