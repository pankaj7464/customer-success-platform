using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam;
namespace Promact.CustomerSuccess.Platform.Services.ApprovedTeams
{
    public class ApprovedTeamService : CrudAppService<ApprovedTeam, ApprovedTeamDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateApprovedTeamDto, CreateUpdateApprovedTeamDto>
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<ApprovedTeam, Guid> _approvedTeamRepository;

        public ApprovedTeamService(IRepository<ApprovedTeam, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _approvedTeamRepository = repository;
        }

        public override async Task<ApprovedTeamDto> CreateAsync(CreateUpdateApprovedTeamDto input)
        {
            var approvedTeamDto = await base.CreateAsync(input);

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Approved Team Created alert",
                Body=Template.GetApproveTeamEmailBody(approvedTeamDto,"Created"),
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return approvedTeamDto;
        }

        public override async Task<ApprovedTeamDto> UpdateAsync(Guid id, CreateUpdateApprovedTeamDto input)
        {
            var approvedTeamDto = await base.UpdateAsync(id, input);

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Approved Team Updated alert",
                Body = Template.GetApproveTeamEmailBody(approvedTeamDto, "Updated"),
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return approvedTeamDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Retrieve approved team to get details before deletion
            var approvedTeam = await _approvedTeamRepository.GetAsync(id);

            // Perform deletion
            await base.DeleteAsync(id);

            // Send email notification
            var projectId = approvedTeam.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Approved Team Deleted alert",
                Body =Template.GetApproveTeamEmailBody(ObjectMapper.Map<ApprovedTeam, ApprovedTeamDto>(approvedTeam),"Deleted"),
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));
        }


        public async Task<List<ApprovedTeamDto>> GetApprovedTeamsByProjectIdAsync(Guid projectId)
        {
            var approvedTeams = await _approvedTeamRepository.GetListAsync(t => t.ProjectId == projectId);
            return ObjectMapper.Map<List<ApprovedTeam>, List<ApprovedTeamDto>>(approvedTeams);
        }

    }
}
