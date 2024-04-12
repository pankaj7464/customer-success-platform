using Promact.CustomerSuccess.Platform.Entities.Constants;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.EscalationMatrix
{
    public class UpdateEscalationMatrix
    {
        public EscalationMatrixLevels Level { get; set; }
        public EscalationType EscalationType { get; set; }
        public Guid ResponsiblePerson { get; set; }
        public Guid ProjectId { get; set; }
    }
}
