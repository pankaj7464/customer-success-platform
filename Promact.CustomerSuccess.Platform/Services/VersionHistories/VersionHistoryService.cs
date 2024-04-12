using Microsoft.EntityFrameworkCore;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos.VersionHistory;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
namespace Promact.CustomerSuccess.Platform.Services.VersionHistories
{
    public class VersionHistoryService : CrudAppService<VersionHistory,
                                VersionHistoryDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateVersionHistoryDto,
                                UpdateVersionHistoryDto>,
                                IVersionHistoryService
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<VersionHistory, Guid> _versionHistoryRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;

        public VersionHistoryService(IRepository<VersionHistory, Guid> repository, IEmailService emailService, IRepository<IdentityUser, Guid> userRepository) : base(repository)
        {
            _emailService = emailService;
            _versionHistoryRepository = repository;
            _userRepository = userRepository;
        }

        public override async Task<VersionHistoryDto> CreateAsync(CreateVersionHistoryDto input)
        {
            var versionHistoryDto = await base.CreateAsync(input);

            // Send email notification

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Version History Created Alert",
                ProjectId = projectId,
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);

            return versionHistoryDto;
        }

        public override async Task<VersionHistoryDto> UpdateAsync(Guid id, UpdateVersionHistoryDto input)
        {
            var versionHistoryDto = await base.UpdateAsync(id, input);

            return versionHistoryDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Send email notification

            var versionHistory = await _versionHistoryRepository.GetAsync(id);
            var projectId = versionHistory.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Version History Created Alert",
                ProjectId = projectId,
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);

            await base.DeleteAsync(id);
        }
        public async Task<List<VersionHistoryDto>> GetVersionHistoriesByProjectIdAsync(Guid projectId)
        {
            var queryable = await _versionHistoryRepository.GetQueryableAsync();
            var versionHistories = await queryable.Where(v => v.ProjectId == projectId)
                .Include(v => v.ApprovedBy)
                .Include(v => v.CreatedBy).ToListAsync();
            return ObjectMapper.Map<List<VersionHistory>, List<VersionHistoryDto>>(versionHistories);
        }

    }
}
