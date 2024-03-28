namespace Promact.CustomerSuccess.Platform.Services.Emailing
{
    public class EmailToStakeHolderDto
    {
        public Guid ProjectId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
