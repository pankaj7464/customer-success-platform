using Promact.CustomerSuccess.Platform.Entities.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.sprint
{
    public class SprintDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }

        public required Guid ProjectId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SprintStatus Status { get; set; }
        public string Comments { get; set; }
        public string Goals { get; set; }
        public int SprintNumber { get; set; }




    }
}
