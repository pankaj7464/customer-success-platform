using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Entities.Constants;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.PhaseMilestone
{
    public class UpdatePhaseMilestoneDto
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public MilestoneOrPhaseStatus Status { get; set; }
    }
}
