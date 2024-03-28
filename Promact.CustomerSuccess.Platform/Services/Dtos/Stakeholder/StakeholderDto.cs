using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.Stakeholder
{
    public class StakeholderDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        [Required]
        public string Name { get; set; }
        public string Email { get; set; }

        public Guid ProjectId { get; set; }

    }
}
