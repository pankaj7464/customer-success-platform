namespace Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam
{
    public class CreateUpdateApprovedTeamDto
    {
        public int NoOfResouces { get; set; }
        public string Role { get; set; }
        public int PhaseNo { get; set; }
        public Guid ProjectId { get; set; }
        public string Duration { get; set; }
        public string Availability { get; set; }
    }
}
