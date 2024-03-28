using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class Project : AuditedEntity<Guid>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        // Foreign key for ABP user
        [ForeignKey("User")]
        public Guid ManagerId { get; set; }
        public virtual User User { get; set; }
    }
}
