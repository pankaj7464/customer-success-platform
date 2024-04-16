using System.ComponentModel.DataAnnotations.Schema;
using Promact.CustomerSuccess.Platform.Entities.Constants;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class AuditHistory : AuditedEntity<Guid>
    {

        public DateTime DateOfAudit { get; set; }

        [ForeignKey(nameof(Reviewer))]
        public Guid ReviewerId { get; set; }


        public SprintStatus Status { get; set; }
        public string ReviewedSection { get; set; }
        public string? CommentOrQueries { get; set; }
        public string? ActionItem { get; set; }

        [ForeignKey(nameof(Project))]
        public required Guid ProjectId { get; set; }

        public virtual ApplicationUser? Reviewer { get; set; }
        public virtual Project? Project { get; set; }
    }
}
