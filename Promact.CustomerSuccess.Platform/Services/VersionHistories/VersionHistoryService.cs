using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.VersionHistory;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;




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
        private readonly IRepository<VersionHistory,Guid> _repository;
        private readonly IRepository<User,Guid> _userRepository;

        public VersionHistoryService(IRepository<VersionHistory, Guid> repository, IEmailService emailService, IRepository<User, Guid> userRepository) : base(repository)
        {
            _emailService = emailService;
            _repository = repository;
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
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

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

            var versionHistory = await _repository.GetAsync(id);
            var projectId = versionHistory.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Version History Created Alert",
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            await base.DeleteAsync(id);
        }
        public async Task<List<VersionHistoryDto>> GetVersionHistoriesByProjectIdAsync(Guid projectId)
        {
            // Fetch all users
            var users = await _userRepository.GetListAsync();

            // Fetch version histories for the specified projectId
            var versionHistories = await _repository.GetListAsync(vh => vh.ProjectId == projectId);

           

            // Map version histories to DTOs
            var versionHistoryDtos = ObjectMapper.Map<List<VersionHistory>, List<VersionHistoryDto>>(versionHistories);

            // If version histories exist
            if (versionHistoryDtos != null)
            {
                // Iterate through each version history
                foreach (var versionHistory in versionHistoryDtos)
                {
                    // Find the user associated with the version history's CreatedBy property
                    versionHistory.Creater = users.FirstOrDefault(u => u.Id == versionHistory.CreatedBy);
                }
            }

            return versionHistoryDtos;
        }

    }
}
