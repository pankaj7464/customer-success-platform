using System.ComponentModel.DataAnnotations;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.User
{
    public class CreateRoleDto
    {
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
