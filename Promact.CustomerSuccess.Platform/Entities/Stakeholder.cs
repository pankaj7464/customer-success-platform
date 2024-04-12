using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class Stakeholder : AuditedEntity<Guid>
    {


        [ForeignKey("Project")]
        public Guid ProjectId { get; set; }

        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual IdentityRole Role { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Project? Project { get; set; }
    }
}
