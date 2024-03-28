using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.EscalationMatrix;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.EscalationMatrices
{
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
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));

            return escalationMatrixDto;
        }

        public override async Task<EscalationMatrixDto> UpdateAsync(Guid id, UpdateEscalationMatrix input)
        {
            var escalationMatrixDto = await base.UpdateAsync(id, input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Escalation Matrix Updated alert",
                Body ="Escalation Matrix Updated "
            };
            _emailService.SendEmail(emailDto);

            return escalationMatrixDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Escalation Matrix Deleted alert",
                Body = "Escalation matrix Deleted "
            };
            _emailService.SendEmail(emailDto);

            await base.DeleteAsync(id);
        }

        public async Task<List<EscalationMatrix>> GetEscalationmatricesByProjectIdAsync(Guid projectId)
        {
            return await _escalationMatrixRepository.GetListAsync(ah => ah.ProjectId == projectId);
        }

    }
}
