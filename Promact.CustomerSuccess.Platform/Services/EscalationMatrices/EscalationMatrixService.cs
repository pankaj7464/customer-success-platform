using Microsoft.AspNetCore.Authorization;
using Promact.CustomerSuccess.Platform.Constants;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.EscalationMatrix;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.EscalationMatrices
{
    [Authorize]
    public class EscalationMatrixService : CrudAppService<EscalationMatrix,
                                EscalationMatrixDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateEscalationMatrix,
                                UpdateEscalationMatrix>,
                                IEscalationMatrixService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail ; 
        private readonly string Username ;
        IRepository<EscalationMatrix, Guid> _escalationMatrixRepository;
        public EscalationMatrixService(IRepository<EscalationMatrix, Guid> escalationMatrixRepository, IEmailService emailService)
            : base(escalationMatrixRepository)
        {
            _emailService = emailService;
            _escalationMatrixRepository = escalationMatrixRepository;


        }


        [Authorize(Policy = PolicyName.EscalationMatrixCreatePolicy)]
        public override async Task<EscalationMatrixDto> CreateAsync(CreateEscalationMatrix input)
        {
            var escalationMatrixDto = await base.CreateAsync(input);

            
            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Escalation Matrix Created alert",
                ProjectId = projectId,
                Body = "Escalation matrix Created please check !"
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);

            return escalationMatrixDto;
        }
        [Authorize(Policy = PolicyName.EscalationMatrixUpdatePolicy)]
        public override async Task<EscalationMatrixDto> UpdateAsync(Guid id, UpdateEscalationMatrix input)
        {
            var escalationMatrixDto = await base.UpdateAsync(id, input);
            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Escalation Matrix Updated alert",
                ProjectId = projectId,
                Body = "Escalation matrix Updated please check !"
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);

            return escalationMatrixDto;
        }

        [Authorize(Policy = PolicyName.EscalationMatrixDeletePolicy)]
        public override async Task DeleteAsync(Guid id)
        {
            var escalation =await base.GetAsync(id);

            var projectId = escalation.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Escalation Matrix Deleted alert",
                ProjectId = projectId,
                Body = "Escalation matrix Deleted please check !"
            };
            await _emailService.SendEmailToStakeHolder(projectDetail);


            await base.DeleteAsync(id);
        }

        public async Task<List<EscalationMatrix>> GetEscalationmatricesByProjectIdAsync(Guid projectId)
        {
            return await _escalationMatrixRepository.GetListAsync(ah => ah.ProjectId == projectId);
        }

    }
}
