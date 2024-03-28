using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectResource;

namespace Promact.CustomerSuccess.Platform.Services.Resource
{
    public class ResourceService : CrudAppService<ProjectResources, ProjectResourcesDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProjectResourceDto, CreateUpdateProjectResourceDto>
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<ProjectResources, Guid> _resourceRepository;

        public ResourceService(IRepository<ProjectResources, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _resourceRepository = repository;
        }

        public override async Task<ProjectResourcesDto> CreateAsync(CreateUpdateProjectResourceDto input)
        {
            var resourceDto = await base.CreateAsync(input);

            // Send email notification

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Resource Created Alert",
                Body = Template.GenerateProjectResourceEmailBody(resourceDto,"Created"),
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return resourceDto;
        }

        public override async Task<ProjectResourcesDto> UpdateAsync(Guid id, CreateUpdateProjectResourceDto input)
        {
            var resourceDto = await base.UpdateAsync(id, input);

            // Send email notification

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Resource Updated Alert",
                Body = Template.GenerateProjectResourceEmailBody(resourceDto,"Updated"),
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return resourceDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Retrieve resource to get details before deletion
            var resource = await _resourceRepository.GetAsync(id);

            // Perform deletion
            await base.DeleteAsync(id);

            // Send email notification
            // Send email notification

            var projectId = resource.ProjectId;


            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Project Update Created Alert",
                ProjectId = projectId,
                Body = "Project resource has been deleted"
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));
 
        }

        public async Task<List<ProjectResourcesDto>> GetResourcesByProjectIdAsync(Guid projectId)
        {
            var resources = await _resourceRepository.GetListAsync(r => r.ProjectId == projectId);
            return ObjectMapper.Map<List<ProjectResources>, List<ProjectResourcesDto>>(resources);
        }
    }
}
