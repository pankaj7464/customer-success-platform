
using Promact.CustomerSuccess.Platform.Services.Dtos.User;
using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.VersionHistory
{
    public class VersionHistoryDto : IEntityDto<Guid>
    {
        public float Version { get; set; }
        public string Type { get; set; }
        public string Change { get; set; }
        public string ChangeReason { get; set; }
        public UserDto? CreatedBy { get; set; }
        public DateTime RevisionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public UserDto? ApprovedBy { get; set; }
        public  Guid ProjectId { get; set; }

        public Guid Id { get; set; }

    }
}
