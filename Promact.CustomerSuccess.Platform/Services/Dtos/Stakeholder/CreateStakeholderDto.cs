namespace Promact.CustomerSuccess.Platform.Services.Dtos.Stakeholder
{
    public class CreateStakeholderDto
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
