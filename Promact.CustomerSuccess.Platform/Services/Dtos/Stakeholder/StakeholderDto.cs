using System.ComponentModel.DataAnnotations;
using Promact.CustomerSuccess.Platform.Services.Dtos.User;
using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.Stakeholder
{
    public class StakeholderDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public RoleDto Role { get; set; }
        public UserDto User { get; set; }
        public Guid ProjectId { get; set; }

    }
}
