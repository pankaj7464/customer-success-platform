using Promact.CustomerSuccess.Platform.Entities.Constants;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class User:AuditedEntity<Guid>
    {
      
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }


    }
}
