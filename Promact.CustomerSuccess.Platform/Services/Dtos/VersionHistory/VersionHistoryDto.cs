using Promact.CustomerSuccess.Platform.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.VersionHistory
{
    public class VersionHistoryDto : IEntityDto<Guid>
    {
        public int Version { get; set; }
        public string Type { get; set; }
        public string Change { get; set; }
        public string ChangeReason { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime RevisionDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
        public required Guid ProjectId { get; set; }

        public IdentityUser Creater { get; set; }
        public Guid Id { get; set; }

    }
}
