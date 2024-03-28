using AutoMapper;
using Promact.CustomerSuccess.Platform.Entities;
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
using CreateUpdateRoleDto = Promact.CustomerSuccess.Platform.Services.Dtos.Auth.CreateUpdateRoleDto;

namespace Promact.CustomerSuccess.Platform.ObjectMapping;

public class PlatformAutoMapperProfile : Profile
{
    public PlatformAutoMapperProfile()
    {
        /* Create your AutoMapper object mappings here */
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
        CreateMap<Project, ProjectDto>().ReverseMap();


        //Done
        CreateMap<CreateStakeholderDto, Stakeholder>();
        CreateMap<UpdateStakeholderDto, Stakeholder>();
        CreateMap<Stakeholder, StakeholderDto>().ReverseMap();


        //Done
        CreateMap<CreateVersionHistoryDto, VersionHistory>();
        CreateMap<UpdateVersionHistoryDto, VersionHistory>();
        CreateMap<VersionHistory, VersionHistoryDto>().ReverseMap();


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
        CreateMap<CreateUpdateUserRole, UserRole>();
        CreateMap<CreateUpdateUserRole, UserRole>();
        CreateMap<UserRole, UserRoleDto>().ReverseMap();

          //Pending
        CreateMap<CreateUpdateUserDto, User>();
        CreateMap<CreateUpdateUserDto, User>();
        CreateMap<User, UserDto>().ReverseMap();

          //Pending
        CreateMap<CreateUpdateRoleDto, Role>();
        CreateMap<CreateUpdateRoleDto, Role>();
        CreateMap<Role, RoleDto>().ReverseMap();











    }
}
