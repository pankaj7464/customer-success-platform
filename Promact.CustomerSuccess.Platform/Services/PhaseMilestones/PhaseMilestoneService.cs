using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.PhaseMilestone;
using Promact.CustomerSuccess.Platform.Services.Emailing; // Import the email service namespace
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.PhaseMilestones
{
    public class PhaseMilestoneService : CrudAppService<PhaseMilestone,
                                PhaseMilestoneDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreatePhaseMilestoneDto,
                                UpdatePhaseMilestoneDto>,
                                IPhaseMilestoneService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail; 
        private readonly string Username;
        IRepository<PhaseMilestone, Guid> _phaseMilestoneRepository;
        public PhaseMilestoneService(IRepository<PhaseMilestone, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _phaseMilestoneRepository = repository;
        }

        public override async Task<PhaseMilestoneDto> CreateAsync(CreatePhaseMilestoneDto input)
        {
            var phaseMilestoneDto = await base.CreateAsync(input);

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Phase Milestone Created alert",
                Body="Phase milestone Created Plase check ",
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return phaseMilestoneDto;
        }

        public override async Task<PhaseMilestoneDto> UpdateAsync(Guid id, UpdatePhaseMilestoneDto input)
        {
            var phaseMilestoneDto = await base.UpdateAsync(id, input);

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Phase milestone Updated alert",
                Body ="Phase mile stone updated please check",
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return phaseMilestoneDto;
        }

        public override async Task DeleteAsync(Guid id)
        {

            var phaseMilestone= await _phaseMilestoneRepository.GetAsync(id);

            var projectId = phaseMilestone.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Approved Team Created alert",
                Body ="Phase milestone deleted !",
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            await base.DeleteAsync(id);
        }


        public async Task<List<PhaseMilestone>> GetPhaseMilestoneByProjectIdAsync(Guid projectId)
        {
            return await _phaseMilestoneRepository.GetListAsync(ah => ah.ProjectId == projectId);
        }

    }
}
