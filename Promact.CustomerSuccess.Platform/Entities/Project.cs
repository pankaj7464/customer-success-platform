
using Promact.CustomerSuccess.Platform.Entities.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class Project : AuditedEntity<Guid>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ProjectStatus status { get; set; }

        [ForeignKey(nameof(Manager))]
        public Guid ManagerId { get; set; }
       public virtual IdentityUser? Manager { get; set; }
    }
}
