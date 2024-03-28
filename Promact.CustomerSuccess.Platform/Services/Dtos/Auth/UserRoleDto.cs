using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.Auth
{
    public class UserRoleDto:IEntityDto<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

    }
}
