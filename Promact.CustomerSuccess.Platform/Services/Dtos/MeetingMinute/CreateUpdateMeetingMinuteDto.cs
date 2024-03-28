namespace Promact.CustomerSuccess.Platform.Services.Dtos.MeetingMinute
{
    public class CreateUpdateMeetingMinuteDto
    {
        public Guid ProjectId { get; set; }
        public required DateTime MeetingDate { get; set; }
        public required string MoMLink { get; set; }
        public required string Comments { get; set; }
    }
}
