using System;
using System.Threading.Tasks;
using AutoMapper;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectBudget;
using Promact.CustomerSuccess.Platform.Services.Emailing; // Import the email service namespace
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.ProjectBudgets
{
    public class ProjectBudgetService : CrudAppService<ProjectBudget,
                                ProjectBudgetDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateProjectBudgetDto,
                                UpdateProjectBudgetDto>,
                                IProjectBudgetService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username; 
        private readonly IRepository<ProjectBudget,Guid> _projectBudgetRepository;
        
        public ProjectBudgetService(IRepository<ProjectBudget, Guid> projectBudgetRepository, IEmailService emailService,IRepository
            <Stakeholder,Guid> stakeholderRepository)
            : base(projectBudgetRepository)
        {
            _emailService = emailService;
            _projectBudgetRepository = projectBudgetRepository;
            

        }

        public override async Task<ProjectBudgetDto> CreateAsync(CreateProjectBudgetDto input)
        {
            var projectBudgetDto = await base.CreateAsync(input);

            // Get the project ID from the input
            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Test",
                Body = Template.GenerateProjectBudgetEmailBody(projectBudgetDto,"Created"),
                ProjectId = projectId,
            };

            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail)) ;

            return projectBudgetDto;
        }

        public override async Task<ProjectBudgetDto> UpdateAsync(Guid id, UpdateProjectBudgetDto input)
        {
            var projectBudgetDto = await base.UpdateAsync(id, input);

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Project Budget Updated alert",
                Body    =   Template.GenerateProjectBudgetEmailBody(projectBudgetDto,"Updated"),
                ProjectId = projectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));
            return projectBudgetDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var projectBudget =await _projectBudgetRepository.GetAsync(id);

            if (projectBudget == null)
            {
                // Handle case where project is not found
                // For example, throw an exception or log an error
                return;
            }

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Project Budget Deleted alert",
                Body = Template.GenerateProjectBudgetEmailBody(ObjectMapper.Map<ProjectBudget, ProjectBudgetDto>(projectBudget), "Deleted"),
                ProjectId = projectBudget.ProjectId,
            };
            Task.Run(() => _emailService.SendEmailToStakeHolder(projectDetail));



            await base.DeleteAsync(id);
        }

        public async Task<List<ProjectBudget>> GetProjectBudgetByProjectId(Guid projectId)
        {
            return await _projectBudgetRepository.GetListAsync(ah => ah.ProjectId == projectId);
        }

    }
}
