
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class VersionHistory : AuditedEntity<Guid>
    {
        public float Version { get; set; }
        public string Type { get; set; }
        public string Change { get; set; }
        public string ChangeReason { get; set; }

        public DateTime RevisionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }


        [ForeignKey(nameof(CreatedBy))]
        public Guid CreatedById { get; set; }
        [ForeignKey(nameof(ApprovedBy))]
        public Guid? ApprovedById { get; set; }

        [ForeignKey(nameof(Project))]
        public required Guid ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public virtual IdentityUser? CreatedBy { get; set; }
        public virtual IdentityUser? ApprovedBy { get; set; }
    }
}
