
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promact.CustomerSuccess.Platform.Constants;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos.Project;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Promact.CustomerSuccess.Platform.Services
{

    [Authorize]
    public class ProjectService : CrudAppService<
                                Project,
                                ProjectDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateProjectDto,
                                UpdateProjectDto>,
                                IProjectService
    {
        private readonly IEmailService _emailService;

        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Stakeholder, Guid> _stakeholderRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        public ProjectService(IRepository<Project, Guid> projectRepository, IEmailService emailService,
            IRepository<Stakeholder, Guid> stakeholderRepository, IRepository<IdentityUser, Guid> userRepository) : base(projectRepository)
        {
            _emailService = emailService;
            _projectRepository = projectRepository;
            _stakeholderRepository = stakeholderRepository;
            _userRepository = userRepository;
        }
        [Authorize(Policy = PolicyName.ProjectCreatePolicy)]
        public override async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            var projectDto = await base.CreateAsync(input);
            return projectDto;
        }

        [Authorize(Policy = PolicyName.ProjectDeletePolicy)]
        public override async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            var projectDto = base.UpdateAsync(id, input);
            return await projectDto;
        }

        [Authorize(Policy = PolicyName.ProjectDeletePolicy)]
        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }


        public async override Task<PagedResultDto<ProjectDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            // Get the current user's ID
            var currentUserId = Guid.NewGuid();
            List<Project> projects = new List<Project>();
            List<string> roles = new List<string>();

            // Check if the array of roles contains a specific role
            if (roles.Any(role => role == "Admin" || role == "Auditor"))
            {
                // Admin or Auditor can see all projects
                var allProjects = await _projectRepository.GetListAsync();
                projects.AddRange(allProjects);
            }

            if (roles.Contains("Manager"))
            {
                // Manager can see projects where they are project managers
                var managerProjects = await _projectRepository.GetListAsync(p => p.ManagerId == currentUserId);
                projects.AddRange(managerProjects);
            }

            if (roles.Contains("Client"))
            {
                // Client can see projects where they are stakeholders

                // Fetch all stakeholders with the specified user ID
                var user = await _userRepository.GetAsync(currentUserId);
                var stakeholders = await _stakeholderRepository
                    .GetListAsync(s => s.Email == user.Email);

                // Fetch projects associated with the retrieved stakeholders
                var projectIds = stakeholders.Select(s => s.ProjectId).ToList();
                var clientProjects = await _projectRepository
                    .GetListAsync(p => projectIds.Contains(p.Id));
                projects.AddRange(clientProjects);
            }

            // Map the fetched entities to DTOs
            var projectDtos = ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects);

            // Return the paged result
            return new PagedResultDto<ProjectDto>
            {
                TotalCount = projects.Count,
                Items = projectDtos
            };
        }

    }
}
