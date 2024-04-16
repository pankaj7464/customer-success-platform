
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Promact.CustomerSuccess.Platform.Constants;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos.Project;
using Promact.CustomerSuccess.Platform.Services.Dtos.Stakeholder;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using System.Security.Claims;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICurrentUser _currentUser;

        public ProjectService(IRepository<Project, Guid> projectRepository, IEmailService emailService,
            ICurrentUser currentUser,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Stakeholder, Guid> stakeholderRepository, IRepository<IdentityUser, Guid> userRepository) : base(projectRepository)
        {
            _emailService = emailService;
            _projectRepository = projectRepository;
            _stakeholderRepository = stakeholderRepository;
            _contextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }
        [Authorize(Policy = PolicyName.ProjectCreatePolicy)]
        public override async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            var projectDto = await base.CreateAsync(input);
            return projectDto;
        }

        [Authorize(Policy = PolicyName.ProjectUpdatePolicy)]
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
            var user = _contextAccessor.HttpContext.User;
            var currentUserId = Guid.NewGuid(); // Assuming default value if user ID is not available
            var projects = new List<Project>();

            if (user.Identity.IsAuthenticated)
            {
                var roles = new List<string>();

                var roleClaim = user.FindFirst(ClaimTypes.Role);

                if (roleClaim != null)
                {
                    var role = roleClaim.Value.ToLower();

                    // Add the current role to the list
                    roles.Add(role);

                    // Retrieve other roles
                    var otherClaims = user.FindAll(ClaimTypes.Role);

                    foreach (var claim in otherClaims)
                    {
                        var otherRole = claim.Value.ToLower();
                        // Add other roles to the list if they are not already added
                        if (!roles.Contains(otherRole))
                        {
                            roles.Add(otherRole);
                        }
                    }

                    // Now, roles list contains all roles the user belongs to
                    // You can perform data filtering based on roles

                    if (roles.Contains("admin") || roles.Contains("auditor"))
                    {
                        // Return all data for admin or auditor
                        var allProjects = await _projectRepository.GetListAsync();
                        projects.AddRange(allProjects);
                    }
                    else if (roles.Contains("manager"))
                    {
                        // Return manager data
                      
                        var queryable = await _projectRepository.GetQueryableAsync();
                        var managerProjects = queryable
                            .Where(p => p.ManagerId == currentUserId)
                            .Include(p => p.Manager);
                        projects.AddRange(managerProjects);
                    }
                    else if (roles.Contains("client"))
                    {
                        // Return client-specific data
                        var emailClaim = user.FindFirst(ClaimTypes.Email);
                        if (emailClaim != null)
                        {
                            var email = emailClaim.Value;
                            var queryable = await _stakeholderRepository.GetQueryableAsync();
                            var stakeholder = await queryable.Include(s => s.User).ToListAsync();

                            var stakeholderDto = ObjectMapper.Map<List<Stakeholder>, List<StakeholderDto>>(stakeholder);
                            var sDto = stakeholderDto.Where(s => s.User.Email == email);

                            var projectIds = sDto.Select(s => s.ProjectId).ToList();
                            var projectQueryable = await _projectRepository.GetQueryableAsync();
                            var clientProjects = projectQueryable.Where(p => projectIds.Contains(p.Id)).Include(p => p.Manager);
                            projects.AddRange(clientProjects);
                        }
                    }
                }
            }

            // Return the paged result
            return new PagedResultDto<ProjectDto>
            {
                TotalCount = projects.Count,
                Items = ObjectMapper.Map<List<Project>,List<ProjectDto>>(projects)
            };
        }

    }
}
