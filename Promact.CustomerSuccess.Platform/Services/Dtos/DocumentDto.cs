using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform
{
    public class DocumentDto : IEntityDto<Guid>
    {
        public required string Url { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }        
        public required Guid ProjectId { get; set; }        
        public Guid Id { get; set; }
    }
}