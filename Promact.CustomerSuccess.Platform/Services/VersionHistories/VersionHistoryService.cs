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

            var versionHistory = await _repository.GetAsync(id);
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
            // Fetch all users
            var users = await _userRepository.GetListAsync();

            // Fetch version histories for the specified projectId
            var versionHistories = await _repository.GetListAsync(vh => vh.ProjectId == projectId);

           

            // Map version histories to DTOs
            var versionHistoryDtos = ObjectMapper.Map<List<VersionHistory>, List<VersionHistoryDto>>(versionHistories);

            // If version histories exist
            // Assuming versionHistoryDtos is a collection of VersionHistoryDto objects
            // and users is a collection of ApplicationUser objects

            // If version histories exist
            if (versionHistoryDtos != null)
            {
                // Create a dictionary for quick lookup of users by their Id
                var userDictionary = users.ToDictionary(u => u.Id);

                // Iterate through each version history
                foreach (var versionHistory in versionHistoryDtos)
                {
                    // Use the dictionary to find the user associated with the version history's CreatedBy property
                    if (userDictionary.TryGetValue(versionHistory.CreatedBy, out var user))
                    {
                        versionHistory.Creater = user;
                    }
                    
                }
            }


            return versionHistoryDtos;
        }

    }
}
