using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class VersionHistory: AuditedEntity<Guid>
    {

        public int Version { get; set; }
        public string Type { get; set; }
        public string Change { get; set; }
        public string ChangeReason { get; set; }

        [ForeignKey(nameof(User))]
        public Guid CreatedBy { get; set; }
        public DateTime RevisionDate { get; set; }

       
        public DateTime? ApprovalDate { get; set; }

        public string ApprovedBy { get; set; }

        [ForeignKey(nameof(Project))]
        public required Guid ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public virtual User? User { get; set; }
    }
}
