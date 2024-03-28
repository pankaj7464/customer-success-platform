using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos.Stakeholder;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.Stakeholders
{
    public class StakeholderService : CrudAppService<Stakeholder,
                                StakeholderDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateStakeholderDto,
                                UpdateStakeholderDto>,
                                IStakeholderService
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<Stakeholder, Guid> _stakeholderRepository;
        public StakeholderService(IRepository<Stakeholder, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _stakeholderRepository = repository;
        }

        public override async Task<StakeholderDto> CreateAsync(CreateStakeholderDto input)
        {
            var stakeholderDto = await base.CreateAsync(input);
            // Send email notification

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Stakeholder Added",
                ProjectId = projectId,
                Body = Template.GetStakeholderEmailBody(stakeholderDto,"Created")
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return stakeholderDto;
        }

        public override async Task<StakeholderDto> UpdateAsync(Guid id, UpdateStakeholderDto input)
        {
            var stakeholderDto = await base.UpdateAsync(id, input);

            // Send email notification

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Stakeholder updated Alert",
                ProjectId = projectId,
                Body = Template.GetStakeholderEmailBody(stakeholderDto,"Updated")
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return stakeholderDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Send email notification

            var  stakeholder = await _stakeholderRepository.GetAsync (id);

            var projectId = stakeholder.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Project Update Deleted Alert",
                ProjectId = projectId,
                Body = Template.GetStakeholderEmailBody(ObjectMapper.Map<Stakeholder, StakeholderDto>(stakeholder), "Deleted"),
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            await base.DeleteAsync(id);
        }

        public async Task<List<StakeholderDto>> GetStakeholdersByProjectIdAsync(Guid projectId)
        {
            var stakeholders = await _stakeholderRepository.GetListAsync(s => s.ProjectId == projectId);
            return ObjectMapper.Map<List<Stakeholder>, List<StakeholderDto>>(stakeholders);
        }
    }
}
