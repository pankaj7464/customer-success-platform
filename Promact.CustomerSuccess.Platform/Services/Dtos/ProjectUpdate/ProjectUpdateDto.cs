using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.ProjectUpdate
{
    public class ProjectUpdateDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }
        public string GeneralUpdate { get; set; }
        public Guid ProjectId { get; set; }
    }
}
