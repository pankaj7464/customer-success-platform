namespace Promact.CustomerSuccess.Platform.Services.Dtos
{
    public class UpdateDocumentDto
    {
        public required string Url { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Guid ProjectId { get; set; }
        public Guid Id { get; set; }
    }
}
