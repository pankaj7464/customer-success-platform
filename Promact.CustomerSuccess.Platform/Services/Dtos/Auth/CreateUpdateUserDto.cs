namespace Promact.CustomerSuccess.Platform.Services.Dtos.Auth
{
    public class CreateUpdateUserDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool active { get; set; }
    }
}
