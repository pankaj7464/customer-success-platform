using System.ComponentModel.DataAnnotations.Schema;
using Promact.CustomerSuccess.Platform.Entities.Constants;
using Volo.Abp.Domain.Entities;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class AuditHistory:Entity<Guid>
    {

        public DateTime DateOfAudit { get; set; }
        [ForeignKey("User")]
        public Guid ReviewedBy { get; set; }
        
        public virtual User? User { get; set; }
        public SprintStatus Status { get; set; }    
        public string ReviewedSection { get; set; }
        public string? CommentOrQueries { get; set; }
        public string? ActionItem { get; set; }

        [ForeignKey(nameof(Project))]
        public required Guid ProjectId { get; set; }
        public virtual Project? Project { get; set; }
    }
}
