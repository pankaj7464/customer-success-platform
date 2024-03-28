using Promact.CustomerSuccess.Platform.Entities.Constants;
using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.EscalationMatrix
{
    public class EscalationMatrixDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public EscalationMatrixLevels Level { get; set; }
        public EscalationType EscalationType { get; set; }

        public string ResponsiblePerson { get; set; }
        public Guid ProjectId { get; set; }
    }
}