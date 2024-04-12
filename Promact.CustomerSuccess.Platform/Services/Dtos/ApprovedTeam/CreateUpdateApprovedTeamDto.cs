namespace Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam
{
    public class CreateUpdateApprovedTeamDto
    {
        public int NoOfResources { get; set; }
        public Guid RoleId { get; set; }
        public int PhaseNo { get; set; }
        public Guid ProjectId { get; set; }
        public string Duration { get; set; }
        public string Availability { get; set; }
    }
}
