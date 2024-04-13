using Promact.CustomerSuccess.Platform.Services.Dtos.EscalationMatrix;
using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.EscalationMatrices
{
    public interface IEscalationMatrixService:IApplicationService
    {
        /// <summary>
        /// Retrieves a list of escalation matrices by project ID asynchronously.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of escalation matrix DTOs.</returns>
        Task<List<EscalationMatrixDto>> GetEscalationmatricesByProjectIdAsync(Guid projectId);
    }
}
