using Promact.CustomerSuccess.Platform.Services.Dtos.User;
using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam
{
    public class ApprovedTeamDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public int NoOfResources { get; set; }
        public int PhaseNo { get; set; }
        public RoleDto Role { get; set; }
        public string Duration { get; set; }
        public string Availability { get; set; }
        public Guid ProjectId { get; set; }
    }
}
