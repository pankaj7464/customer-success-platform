
using Microsoft.AspNetCore.Authorization;
using Promact.CustomerSuccess.Platform.Constants;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos.AuditHistory;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.AuditHistories
{

    [Authorize]
    public class AuditHistoryService : CrudAppService<AuditHistory,
                       AuditHistoryDto,
                       Guid,
                       PagedAndSortedResultRequestDto,
                       CreateAuditHistoryDto,
                       UpdateAuditHistoryDto>,
                       IAuditHistoryService
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IRepository<AuditHistory, Guid> _auditHistoryRepository;

        public AuditHistoryService(IRepository<AuditHistory, Guid> auditHistoryRepository, IEmailService emailService, IRepository<User, Guid> userRepository)
            : base(auditHistoryRepository)
        {
            _emailService = emailService;
            _auditHistoryRepository = auditHistoryRepository;
            _userRepository = userRepository;
        }

        [Authorize(Policy = PolicyName.AuditHistoryCreatePolicy)]
        public override async Task<AuditHistoryDto> CreateAsync(CreateAuditHistoryDto input)
        {
            var auditHistoryDto = await base.CreateAsync(input);

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Audit Created alert",
                Body = Template.GetAuditHistoryEmailBody(auditHistoryDto, "Created"),
                ProjectId = projectId,
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);
         

            return auditHistoryDto;
        }

        [Authorize(Policy = PolicyName.AuditHistoryUpdatePolicy)]
        public override async Task<AuditHistoryDto> UpdateAsync(Guid id, UpdateAuditHistoryDto input)
        {
            var auditHistoryDto = await base.UpdateAsync(id, input);

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Audit Created alert",
                Body = Template.GetAuditHistoryEmailBody(auditHistoryDto,"Updated"),
                ProjectId = projectId,

            };
            await _emailService.SendEmailToStakeHolder(projectDetail);

            return auditHistoryDto;
        }

        [Authorize(Policy = PolicyName.AuditHistoryDeletePolicy)]
        public override async Task DeleteAsync(Guid id)
        {
            var auditnHistory = await _auditHistoryRepository.GetAsync(id);
            var projectId = auditnHistory.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Audit History Deleted alert ",
                ProjectId = projectId,
                Body = Template.GetAuditHistoryEmailBody(ObjectMapper.Map<AuditHistory, AuditHistoryDto>(auditnHistory), "Deleted"),
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);

            await base.DeleteAsync(id);
        }


        public override Task<PagedResultDto<AuditHistoryDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return base.GetListAsync(input);
        }
        public async Task<List<AuditHistoryDto>> GetAuditHistoriesByProjectIdAsync(Guid projectId)
        {

            // Fetch all users
            var users = await _userRepository.GetListAsync();

            // Fetch version histories for the specified projectId
            var auditHistories = await _auditHistoryRepository.GetListAsync(vh => vh.ProjectId == projectId);



            // Map version histories to DTOs
            var auditHistoryDtos = ObjectMapper.Map<List<AuditHistory>, List<AuditHistoryDto>>(auditHistories);

            // If version histories exist and there are users
            if (auditHistoryDtos != null && users != null)
            {
                // Create a dictionary for faster lookup
                var userDictionary = users.ToDictionary(u => u.Id);

                // Iterate through each version history
                foreach (var auditHistoryDto in auditHistoryDtos)
                {
                    // Find the user associated with the version history's ReviewedBy property
                    if (userDictionary.TryGetValue(auditHistoryDto.ReviewedBy, out var user))
                    {
                        auditHistoryDto.ReviewedByUser = user;
                    }
                }
            }

            return auditHistoryDtos;
        }


    }
}
