using Promact.CustomerSuccess.Platform.Entities.Constants;
using Volo.Abp.Domain.Entities.Auditing;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class Project : AuditedEntity<Guid>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ProjectStatus status { get; set; }
        public Guid ManagerId { get; set; }
    }
}
