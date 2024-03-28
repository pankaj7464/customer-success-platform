using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam
{
    public class ApprovedTeamDto : IEntityDto<Guid>
    {
        internal readonly object NoOfResources;

        public Guid Id { get; set; }
        public int NoOfResouces { get; set; }
        public int PhaseNo { get; set; }
        public string Role { get; set; }
        public string Duration { get; set; }
        public string Availability { get; set; }
        public Guid ProjectId { get; set; }
    }
}
