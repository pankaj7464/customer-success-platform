
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class ApplicationRole: IdentityRole
    {
        public string Name {  get; set; }
        public string Description { get; set; }

       public ApplicationRole(string Name ,string Description) : base(Guid.NewGuid(), Name)
        {
            this.Description = Description;
        }
    }
}
