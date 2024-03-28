using Castle.Core.Smtp;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam;
using Promact.CustomerSuccess.Platform.Services.Dtos.sprint;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.Sprints
{
    public class SprintService : CrudAppService<Sprint, SprintDto, Guid,
        PagedAndSortedResultRequestDto, CreateSprintDto, UpdateSprintDto>
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username ;
        private readonly IRepository<Sprint, Guid> _sprintRepository;

        public SprintService(IRepository<Sprint, Guid> repository,IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _sprintRepository = repository;
        }


        public override async Task<SprintDto> CreateAsync(CreateSprintDto input)
        {
          
            var sprintDto = await base.CreateAsync(input);

            // Send email notification

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Sprint Created  Alert",
                ProjectId = projectId,
                Body = Template.GetSprintEmailBody(sprintDto, "Created"),
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return sprintDto;
        }

        public override async Task<SprintDto> UpdateAsync(Guid id, UpdateSprintDto input)
        {
            var sprintDto = await base.UpdateAsync(id, input);

            // Send email notification

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Sprint Updated Alert",
                ProjectId = projectId,
                Body = Template.GetSprintEmailBody(sprintDto, "Updated"),
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return sprintDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // You can perform any additional operations before deleting the entity if needed
            // Send email notification
            var sprint = await _sprintRepository.GetAsync(id);

            var projectId = sprint.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Project Update Created Alert",
                ProjectId = projectId,
                Body = Template.GetSprintEmailBody(ObjectMapper.Map<Sprint, SprintDto>(sprint), "Deleted"),
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));
            await base.DeleteAsync(id);
        }
        public async Task<List<SprintDto>> GetSprintsByProjectIdAsync(Guid projectId)
        {
            var sprints = await _sprintRepository.GetListAsync(s => s.ProjectId == projectId);
            return ObjectMapper.Map<List<Sprint>, List<SprintDto>>(sprints);
        }



    }
}
