using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Data;
namespace Promact.CustomerSuccess.Platform.Entities
{

    public class ApplicationUser : IdentityUser<Guid> { 
        public required string Name { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public  string? Surname { get; set; }

        public bool IsActive { get; set; }

        public bool EmailConfirmed { get; set; }

        public  string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public Guid? TenantId { get; set; }



        [NotMapped]
        public ExtraPropertyDictionary? ExtraProperties { get; set; }

     
    }
}
