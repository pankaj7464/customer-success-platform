using Microsoft.AspNetCore.Mvc;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos.Project;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using static Volo.Abp.UI.Navigation.DefaultMenuNames.Application;

namespace Promact.CustomerSuccess.Platform.Services
{
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
        
        private readonly IRepository<Project,Guid> _projectRepository;
        private readonly IRepository<Role,Guid> _roleRepository;
        private readonly IRepository<Stakeholder,Guid> _stakeholderRepository;
        private readonly IRepository<UserRole,Guid> _userRoleRepository;
        private readonly IRepository<User,Guid> _userRepository;
        public ProjectService(IRepository<Project, Guid> projectRepository,IEmailService emailService, IRepository<Role, Guid> roleRepository,
            IRepository<Stakeholder, Guid> stakeholderRepository, IRepository<UserRole, Guid> userRoleRepository, IRepository<User, Guid> userRepository) : base(projectRepository)
        {
            _emailService = emailService;
            _projectRepository = projectRepository;
            _roleRepository = roleRepository;
            _stakeholderRepository = stakeholderRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
        }
        public override async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            var projectDto = await base.CreateAsync(input);


            return projectDto;
        }

        public override async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            var projectDto =  base.UpdateAsync(id, input);


            return await projectDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            await base.DeleteAsync(id);
        }

        [HttpGet("projects")]
        public async Task<List<ProjectDto>> GetProjectsByUserIdAsync(Guid userId)
        {
            var projects = new List<Project>();
                var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == userId);
                var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
            if (userRole != null)
            {
                switch (role.Name)
                {
                    case "Admin":
                    case "Auditor":
                        // Admin or Auditor can see all projects
                        projects = await _projectRepository.GetListAsync();
                        break;

                    case "Manager":
                        // Manager can see projects where they are project managers
                        projects = await _projectRepository.GetListAsync(p => p.ManagerId == userId);
                        break;

                    case "Client":
                        // Client can see projects where they are stakeholders

                        // Fetch all stakeholders with the specified email
                        var user = await _userRepository.GetAsync(userId);
                        if (user == null)
                        {
                            return null;
                        }
                        var stakeholders = await _stakeholderRepository
                             .GetListAsync(s => s.Email == user.Email);

                        // Fetch projects associated with the retrieved stakeholders
                        var projectIds = stakeholders.Select(s => s.ProjectId).ToList();
                        projects = await _projectRepository
                         .GetListAsync(p => projectIds.Contains(p.Id));

                        break;

                    default:
                        // Handle other roles as needed
                        break;
                }
            }
            else return null;
            return ObjectMapper.Map<List<Project>, List<ProjectDto>>(projects);

        }
          

       
    }
}
