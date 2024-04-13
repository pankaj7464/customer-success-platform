using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.MeetingMinute;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Promact.CustomerSuccess.Platform.Services.MeetingMinutes
{
    public interface IMeetingMinuteService:IApplicationService
    {
        /// <summary>
        /// Retrieves a list of meeting minutes by project ID asynchronously.
        /// </summary>
        /// <param name="projectId">The unique identifier of the project.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of meeting minute DTOs.</returns>
        Task<List<MeetingMinuteDto>> GetMeetingMinuteByProjectIdAsync(Guid projectId);
    }
}
