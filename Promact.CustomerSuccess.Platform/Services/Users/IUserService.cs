
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.Users
{
    /// <summary>
    /// Service interface for managing users and roles.
    /// </summary>
    public interface IUserService:IApplicationService
    {
        /// <summary>
        /// Creates a new user with the provided details.
        /// </summary>
        Task<UserDto> CreateAsync(CreateUpdateUserDto input);

        /// <summary>
        /// Updates the user with the specified ID with the provided details.
        /// </summary>
        Task<UserDto> UpdateAsync(Guid id, CreateUpdateUserDto input);

        /// <summary>
        /// Deletes the user with the specified ID.
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        Task<Response> GetAllRolesAsync();

        /// <summary>
        /// Retrieves all users with their roles.
        /// </summary>
        Task<Response> GetAllUsersWithRolesAsync();

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        Task<Response> GetAllUsersAsync();

        /// <summary>
        /// Creates a new role with the specified name.
        /// </summary>
        ValueTask<Response> CreateRoleAsync(string  roleName);

        /// <summary>
        /// Retrieves all users in the specified role.
        /// </summary>
        Task<Response> GetUsersByRoleAsync(string roleName);

        /// <summary>
        /// Update roles of a user.
        /// </summary>
        Task<Response> UpdateRolesAsync(UserRoleDto model);

        /// <summary>
        /// Deletes the role with the specified ID.
        /// </summary>
        Task<Response> DeleteRoleAsync(string roleId);

    }

}
