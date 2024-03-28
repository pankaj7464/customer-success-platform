using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.Auth
{
    public class RoleService : CrudAppService<Role, RoleDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateRoleDto, CreateUpdateRoleDto>, IRoleService
    {
        public RoleService(IRepository<Role, Guid> repository) : base(repository)
        {
        }
    }
}
