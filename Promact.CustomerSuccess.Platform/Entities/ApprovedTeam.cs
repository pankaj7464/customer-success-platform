using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class ApprovedTeam : FullAuditedEntity<Guid>
    {
        public int NoOfResources { get; set; }


        public int PhaseNo { get; set; }
        public string Duration { get; set; }
        public string Availability { get; set; }

        [ForeignKey(nameof(Role))]
        public Guid RoleId { get; set; }

        [ForeignKey(nameof(Project))]
        public required Guid ProjectId { get; set; }

        public virtual ApplicationRole? Role { get; set; }
        public virtual Project? Project { get; set; }

    }
}
