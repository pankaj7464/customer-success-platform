using Promact.CustomerSuccess.Platform.Entities.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class Project : AuditedEntity<Guid>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ProjectStatus status { get; set; }
        [ForeignKey(nameof(Manager))]
        public Guid ManagerId { get; set; }
       public virtual ApplicationUser? Manager { get; set; }
    }
}
