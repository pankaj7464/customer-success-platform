using Promact.CustomerSuccess.Platform.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.VersionHistory
{
    public class CreateVersionHistoryDto
    {
        public float Version { get; set; }
        public string Type { get; set; }
        public string Change { get; set; }
        public string ChangeReason { get; set; }
        public UserDto? CreatedBy { get; set; }
        public DateTime RevisionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public UserDto? ApprovedBy { get; set; }

        public required Guid ProjectId { get; set; }

    }
}
