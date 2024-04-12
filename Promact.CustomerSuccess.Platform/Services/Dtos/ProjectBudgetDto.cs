using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos
{
    public class ProjectBudgetDto : EntityDto<Guid>
    {
        public ProjectType Type { get; set; }
        public int? DurationInMonths { get; set; }
        public int? ContractDuration { get; set; }
        public int? BudgetedHours { get; set; }
        public required double BudgetedCost { get; set; }
        public required string Currency { get; set; }        
        public Guid ProjectId { get; set; }        
    }
}