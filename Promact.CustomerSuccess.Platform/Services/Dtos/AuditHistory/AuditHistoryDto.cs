using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Entities.Constants;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Identity;
namespace Promact.CustomerSuccess.Platform.Services.Dtos.AuditHistory
{
    public class AuditHistoryDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public DateTime DateOfAudit { get; set; }
        public SprintStatus Status { get; set; }
        public string ReviewedSection { get; set; }
        public string? CommentOrQueries { get; set; }
        public UserDto? Reviewer { get; set; }
        public Guid ProjectId { get; set; }
        public string? ActionItem { get; set; }
    }
}
