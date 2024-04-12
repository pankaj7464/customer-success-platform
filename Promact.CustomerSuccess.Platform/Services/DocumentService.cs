using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services
{
    public class DocumentService : CrudAppService<Document, DocumentDto, Guid, PagedAndSortedResultRequestDto, CreateDocumentDto, UpdateDocumentDto>
    {
        public DocumentService(IRepository<Document, Guid> repository) : base(repository) { }
    }
}
