using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.AuditHistory;
using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.AuditHistories
{
    public interface IAuditHistoryService:IApplicationService
    {
       
        /// <summary>
        /// Retrieves a list of audit histories by project ID asynchronously.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project.</param>
        /// <returns> The task result contains a list of audit history DTOs.</returns>
        Task<List<AuditHistoryDto>> GetAuditHistoriesByProjectIdAsync(Guid projectId);
    }
}
