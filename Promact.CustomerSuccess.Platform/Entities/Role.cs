using Volo.Abp.Domain.Entities.Auditing;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class Role : AuditedEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
