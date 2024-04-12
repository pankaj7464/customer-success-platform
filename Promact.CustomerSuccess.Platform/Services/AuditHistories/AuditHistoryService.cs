
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Promact.CustomerSuccess.Platform.Constants;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam;
using Promact.CustomerSuccess.Platform.Services.Dtos.AuditHistory;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

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
        private readonly IRepository<AuditHistory, Guid> _auditHistoryRepository;

        public AuditHistoryService(IRepository<AuditHistory, Guid> auditHistoryRepository, IEmailService emailService, IRepository<IdentityUser, Guid> userRepository)
            : base(auditHistoryRepository)
        {
            _emailService = emailService;
            _auditHistoryRepository = auditHistoryRepository;
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
                Body = Template.GetAuditHistoryEmailBody(auditHistoryDto, "Updated"),
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
            var queryable = await _auditHistoryRepository.GetQueryableAsync();
            var auditHistories = await queryable
                .Where(a => a.ProjectId == projectId)
                .Include(a => a.Reviewer)
                .Select(a => new AuditHistoryDto
                {
                    Id = a.Id,
                    DateOfAudit = a.DateOfAudit,
                    Status = a.Status,
                    ReviewedSection = a.ReviewedSection,
                    CommentOrQueries = a.CommentOrQueries,
                    ActionItem = a.ActionItem,
                    Reviewer = new UserDto
                    {
                        Name = a.Reviewer.Name,
                        Id = a.Reviewer.Id,
                    }
                }).ToListAsync();

            return auditHistories;
        }



    }
}
