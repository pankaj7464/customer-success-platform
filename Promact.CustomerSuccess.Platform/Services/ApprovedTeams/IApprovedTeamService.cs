using Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.ApprovedTeams
{
    public interface IApprovedTeamService
    {
        /// <summary>
        /// Interface for managing approved teams.
        /// </summary>
        public interface IApprovedTeamService : ICrudAppService<
            ApprovedTeamDto, // DTO used to represent approved teams
            Guid, // Primary key type of the approved team entity (Guid in this case)
            PagedAndSortedResultRequestDto, // DTO used for paging and sorting requests
            CreateUpdateApprovedTeamDto, // DTO used for creating approved teams
            CreateUpdateApprovedTeamDto> // DTO used for updating approved teams
        {
           
            /// <summary>
            /// Retrieves a list of approved teams by project ID asynchronously.
            /// </summary>
            /// <param name="projectId">The unique identifier of the project.</param>
            /// <returns>A task representing the asynchronous operation. The task result contains a list of approved team DTOs.</returns>
            Task<List<ApprovedTeamDto>> GetApprovedTeamsByProjectIdAsync(Guid projectId);
        }
    }
}