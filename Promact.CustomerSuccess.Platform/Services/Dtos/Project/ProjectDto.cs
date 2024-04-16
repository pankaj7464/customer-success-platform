
using Promact.CustomerSuccess.Platform.Entities.Constants;
using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.Project
{
    public class ProjectDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ProjectStatus status { get; set; }
        public UserDto Manager { get; set; }
        public required Guid ManagerId { get; set; }
    }
}
