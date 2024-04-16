namespace Promact.CustomerSuccess.Platform.Services.Dtos
{
    public class UserWithRolesDto
    {

        public Guid UserId { get;  set; }
        public string Email { get;  set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get;  set; }
    }
}
