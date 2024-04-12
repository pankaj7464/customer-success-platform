﻿using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam;
using Microsoft.AspNetCore.Authorization;
using Promact.CustomerSuccess.Platform.Constants;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;
using System.Collections.Generic;
using System.Linq;
using Promact.CustomerSuccess.Platform.Services.Dtos;
namespace Promact.CustomerSuccess.Platform.Services.ApprovedTeams
{
    [Authorize]
    public class ApprovedTeamService : CrudAppService<ApprovedTeam, ApprovedTeamDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateApprovedTeamDto, CreateUpdateApprovedTeamDto>
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<ApprovedTeam, Guid> _approvedTeamRepository;

        public ApprovedTeamService(IRepository<ApprovedTeam, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _approvedTeamRepository = repository;
        }

        [Authorize(Policy = PolicyName.ApproveTeamCreatePolicy)]
        public override async Task<ApprovedTeamDto> CreateAsync(CreateUpdateApprovedTeamDto input)
        {
            var approvedTeamDto = await base.CreateAsync(input);

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Approved Team Created alert",
                Body = Template.GetApproveTeamEmailBody(approvedTeamDto, "Created"),
                ProjectId = projectId,
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);

            return approvedTeamDto;
        }

        [Authorize(Policy = PolicyName.ApproveTeamUpdatePolicy)]
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
            await _emailService.SendEmailToStakeHolder(projectDetail);

            return approvedTeamDto;
        }

        [Authorize(Policy = PolicyName.ApproveTeamDeletePolicy)]
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
                Body = Template.GetApproveTeamEmailBody(ObjectMapper.Map<ApprovedTeam, ApprovedTeamDto>(approvedTeam), "Deleted"),
                ProjectId = projectId,
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);
        }
        public async Task<List<ApprovedTeamDto>> GetApprovedTeamsByProjectIdAsync(Guid projectId)
        {
            var queryable = await _approvedTeamRepository.GetQueryableAsync();
            var approvedTeams = await queryable
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Role)
                .Select(t => new ApprovedTeamDto
                {
                    Id = t.Id,
                    NoOfResources = t.NoOfResources,
                    PhaseNo = t.PhaseNo,
                    Role = new RoleDto
                    {
                        Id = t.Role.Id,
                        Name = t.Role.Name
                    },
                    Duration = t.Duration,
                    Availability = t.Availability,
                    ProjectId = t.ProjectId
                })
                .ToListAsync();

            return approvedTeams;
        }




    }
}
