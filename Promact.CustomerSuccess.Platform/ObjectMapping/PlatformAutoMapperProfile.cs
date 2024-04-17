using AutoMapper;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam;
using Promact.CustomerSuccess.Platform.Services.Dtos.AuditHistory;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.Auth.Auth;
using Promact.CustomerSuccess.Platform.Services.Dtos.ClientFeedback;
using Promact.CustomerSuccess.Platform.Services.Dtos.EscalationMatrix;
using Promact.CustomerSuccess.Platform.Services.Dtos.MeetingMinute;
using Promact.CustomerSuccess.Platform.Services.Dtos.PhaseMilestone;
using Promact.CustomerSuccess.Platform.Services.Dtos.Project;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectBudget;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectResource;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectUpdate;
using Promact.CustomerSuccess.Platform.Services.Dtos.RiskProfile;
using Promact.CustomerSuccess.Platform.Services.Dtos.sprint;
using Promact.CustomerSuccess.Platform.Services.Dtos.Stakeholder;
using Promact.CustomerSuccess.Platform.Services.Dtos.VersionHistory;
using Volo.Abp.Identity;
using RoleDto = Promact.CustomerSuccess.Platform.Services.Dtos.RoleDto;

namespace Promact.CustomerSuccess.Platform.ObjectMapping;

public class PlatformAutoMapperProfile : Profile
{
    public PlatformAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */
        /* Project */
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
        CreateMap<Project, ProjectDto>().ReverseMap()
        .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager));


        //Done
        CreateMap<CreateStakeholderDto, Stakeholder>();
        CreateMap<UpdateStakeholderDto, Stakeholder>();
        CreateMap<Stakeholder, StakeholderDto>().ReverseMap()
       .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
       .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));



        //Done
        CreateMap<CreateVersionHistoryDto, VersionHistory>();
        CreateMap<UpdateVersionHistoryDto, VersionHistory>();
        CreateMap<VersionHistory, VersionHistoryDto>().ReverseMap()
       .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
       .ForMember(dest => dest.ApprovedBy, opt => opt.MapFrom(src => src.ApprovedBy));



        //Done
        CreateMap<CreateAuditHistoryDto, AuditHistory>();
        CreateMap<UpdateAuditHistoryDto, AuditHistory>();
        CreateMap<AuditHistory, AuditHistoryDto>().ReverseMap();


        //Done
        CreateMap<CreateProjectBudgetDto, ProjectBudget>();
        CreateMap<UpdateProjectBudgetDto, ProjectBudget>();
        CreateMap<ProjectBudget, ProjectBudgetDto>().ReverseMap();

        //Done
        CreateMap<CreateEscalationMatrix, EscalationMatrix>();
        CreateMap<UpdateEscalationMatrix, EscalationMatrix>();
        CreateMap<EscalationMatrix, EscalationMatrixDto>().ReverseMap();


        //Done
        CreateMap<CreateRiskProfileDto, RiskProfile>();
        CreateMap<UpdateRiskProfileDto, RiskProfile>();
        CreateMap<RiskProfile, RiskProfileDto>().ReverseMap();


        //Done
        CreateMap<CreateSprintDto, Sprint>();
        CreateMap<UpdateSprintDto, Sprint>();
        CreateMap<Sprint, SprintDto>().ReverseMap();

        //Done
        CreateMap<CreatePhaseMilestoneDto, PhaseMilestone>();
        CreateMap<UpdatePhaseMilestoneDto, PhaseMilestone>();
        CreateMap<PhaseMilestone, PhaseMilestoneDto>().ReverseMap();



        //Pending
        CreateMap<CreateUpdateProjectResourceDto, ProjectResources>();
        CreateMap<CreateUpdateProjectResourceDto, ProjectResources>();
        CreateMap<ProjectResources, ProjectResourcesDto>().ReverseMap();

        //Pending
        CreateMap<CreateUpdateMeetingMinuteDto, MeetingMinute>();
        CreateMap<CreateUpdateMeetingMinuteDto, MeetingMinute>();
        CreateMap<MeetingMinute, MeetingMinuteDto>().ReverseMap();

        //Pending
        CreateMap<CreateUpdateApprovedTeamDto, ApprovedTeam>();
        CreateMap<CreateUpdateApprovedTeamDto, ApprovedTeam>();
        CreateMap<ApprovedTeam, ApprovedTeamDto>().ReverseMap();

        //Pending
        CreateMap<CreateUpdateProjectUpdateDto, ProjectUpdate>();
        CreateMap<CreateUpdateProjectUpdateDto, ProjectUpdate>();
        CreateMap<ProjectUpdate, ProjectUpdateDto>().ReverseMap();

        //Pending
        CreateMap<CreateUpdateCLientFeedback, ClientFeedback>();
        CreateMap<CreateUpdateCLientFeedback, ClientFeedback>();
        CreateMap<ClientFeedback, ClientFeedbackDto>().ReverseMap();


        //Pending
        CreateMap<CreateUpdateUserDto, IdentityUser>();
        CreateMap<CreateUpdateUserDto, IdentityUser>();
        CreateMap<IdentityUser, UserWithRoleDto>().ReverseMap();



        CreateMap<IdentityUser, UserDto>().ReverseMap();
        CreateMap<IdentityRole, RoleDto>().ReverseMap();

        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<ApplicationRole, RoleDto>().ReverseMap();



        /* Document */
        CreateMap<CreateDocumentDto, Document>();
        CreateMap<UpdateDocumentDto, Document>();
        CreateMap<Document, DocumentDto>().ReverseMap();

    }
}
