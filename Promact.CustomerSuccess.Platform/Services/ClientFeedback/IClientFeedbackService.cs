using Promact.CustomerSuccess.Platform.Services.Dtos.ClientFeedback;
using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.ClientFeedbacks
{
    public interface IClientFeedbackService:IApplicationService
    {
        /// <summary>
        /// Retrieves a list of client feedbacks by project ID asynchronously.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of client feedback DTOs.</returns>
        Task<List<ClientFeedbackDto>> GetClientFeedbackByProjectIdAsync(Guid projectId);
    }
}
