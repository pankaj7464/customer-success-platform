using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.MeetingMinute;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.MeetingMinutes
{
    public class MeetingMinuteService : CrudAppService<MeetingMinute, 
        MeetingMinuteDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateMeetingMinuteDto, CreateUpdateMeetingMinuteDto>, IMeetingMinuteService
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<MeetingMinute, Guid> _meetingMinuteRepository;

        public MeetingMinuteService(IRepository<MeetingMinute, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _meetingMinuteRepository = repository;
        }

        public override async Task<MeetingMinuteDto> CreateAsync(CreateUpdateMeetingMinuteDto input)
        {
            var meetingMinuteDto = await base.CreateAsync(input);

            // Send email notification
           

            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Minute meeting Created alert",
                Body = Template.GenerateMeetingMinutesEmailBody(meetingMinuteDto, "Created"),
                ProjectId = projectId,
            };
             await _emailService.SendEmailToStakeHolder(projectDetail);

            return meetingMinuteDto;
        }

        public override async Task<MeetingMinuteDto> UpdateAsync(Guid id, CreateUpdateMeetingMinuteDto input)
        {
            var meetingMinuteDto = await base.UpdateAsync(id, input);

            // Send email notification
       
            var projectId = input.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Meeting Minute Updated Alert",
                Body = Template.GenerateMeetingMinutesEmailBody(meetingMinuteDto, "Updated"),
                ProjectId = projectId,
            };
           await _emailService.SendEmailToStakeHolder(projectDetail);

            return meetingMinuteDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Retrieve meeting minute to get details before deletion
            var meetingMinute = await _meetingMinuteRepository.GetAsync(id);

            // Perform deletion
            await base.DeleteAsync(id);

            // Send email notification

            var projectId = meetingMinute.ProjectId;

            var projectDetail = new EmailToStakeHolderDto
            {
                Subject = "Meeting Minute Deleted Alert",
                Body = Template.GenerateMeetingMinutesEmailBody(ObjectMapper.Map<MeetingMinute, MeetingMinuteDto>(meetingMinute), "Deleted"),
                ProjectId = projectId,
            };
           await _emailService.SendEmailToStakeHolder(projectDetail);
        }

        public async Task<List<MeetingMinuteDto>> GetMeetingMinuteByProjectIdAsync(Guid projectId)
        {
            var meetingMinute = await _meetingMinuteRepository.GetListAsync(ah => ah.ProjectId == projectId);

            return ObjectMapper.Map<List<MeetingMinute>, List<MeetingMinuteDto>>(meetingMinute);
        }

    }
}
