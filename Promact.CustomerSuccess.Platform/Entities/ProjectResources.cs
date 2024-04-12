
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class ProjectResources : AuditedEntity<Guid>
    {

        public double AllocationPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }

        [ForeignKey("Project")]
        public Guid ProjectId { get; set; }

        public virtual IdentityRole Role { get; set; }
        public virtual Project? Project { get; set; }


    }
}