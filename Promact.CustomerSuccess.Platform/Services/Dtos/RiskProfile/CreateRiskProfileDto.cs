using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Entities.Constants;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.RiskProfile
{
    public class CreateRiskProfileDto
    {
        public Guid ProjectId { get; set; }
        public RiskType Type { get; set; }
        public RiskSeverity Severity { get; set; }
        public RiskImpact Impact { get; set; }
    }
}
