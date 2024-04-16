using System.ComponentModel.DataAnnotations.Schema;
using Promact.CustomerSuccess.Platform.Entities.Constants;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class EscalationMatrix : AuditedEntity<Guid>
    {
        
        public EscalationMatrixLevels Level { get; set; }
        public EscalationType EscalationType { get; set; }     
        
        [ForeignKey("Project")]
        public required Guid ProjectId { get; set; }

        [ForeignKey((nameof(ResponsiblePerson)))]
        public Guid ResponsiblePersonId { get; set; }
        public virtual Project? Project { get; set; }        
        public virtual ApplicationUser? ResponsiblePerson { get; set; }  
    }
}