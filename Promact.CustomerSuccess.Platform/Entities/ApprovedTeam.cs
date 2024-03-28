using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class ApprovedTeam :AuditedEntity<Guid>
    {
        public int NoOfResouces { get; set; }
        public string Role {  get; set; }
        public int PhaseNo { get; set; }
        public string Duration { get; set; }
        public string Availability {  get; set; }

        [ForeignKey(nameof(Project))]
        public required Guid ProjectId { get; set; }
        public virtual Project? Project { get; set; }

    }
}
