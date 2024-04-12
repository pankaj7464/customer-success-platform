using System.ComponentModel.DataAnnotations.Schema;
using Promact.CustomerSuccess.Platform.Entities.Constants;
using Volo.Abp.Domain.Entities.Auditing;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class ProjectBudget : AuditedEntity<Guid>
    {
        public ProjectType Type { get; set; }
        public int? DurationInMonths { get; set; }
        public int? ContractDuration { get; set; }
        public int? BudgetedHours { get; set; }
        public required double BudgetedCost { get; set; }
        public required string Currency { get; set; }
        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public virtual Project? Project { get; set; }

    }
}
