using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.MeetingMinute
{
    public class MeetingMinuteDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public required DateTime MeetingDate { get; set; }
        public required string MoMLink { get; set; }
        public required string Comments { get; set; }
    }
}