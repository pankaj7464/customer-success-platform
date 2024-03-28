using Microsoft.EntityFrameworkCore;
using Promact.CustomerSuccess.Platform.Entities;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Promact.CustomerSuccess.Platform.Data;

public class PlatformDbContext : AbpDbContext<PlatformDbContext>
{
    public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProjectResources> Resources { get; set; } /*Added*/
    public DbSet<ProjectUpdate> ProjectUpdate { get; set; } /*added*/
    public DbSet<ApprovedTeam> ApprovedTeams { get; set; } /*added*/

    public DbSet<Stakeholder> Stakeholders    { get; set; }
    public DbSet<VersionHistory> VersionHistories    { get; set; }
    public DbSet<AuditHistory> AuditHistories    { get; set; }
    public DbSet<Project> Projects { get; set; }

    public DbSet<ClientFeedback> ClientFeedbacks { get; set; }
    public DbSet<ProjectBudget> ProjectBudgets { get; set; }
   
    public DbSet<PhaseMilestone> PhaseMilestones { get; set; }
    public DbSet<ProjectResources> ProjectResources { get; set; }
    public DbSet<RiskProfile> RiskProfiles { get; set; }
    public DbSet<MeetingMinute> MeetingMinutes { get; set; }
    public DbSet<EscalationMatrix> EscalationMatrices { get; set; }
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own entities here */
        builder.Entity<ProjectResources>(resouces =>
        {
            resouces.ConfigureByConvention();
        });
        builder.Entity<ProjectUpdate>(projectUpdate =>
        {
            projectUpdate.ConfigureByConvention();
        });
        builder.Entity<ApprovedTeam>(approveTeam =>
        {
            approveTeam.ConfigureByConvention();
        });
        builder.Entity<AuditHistory>(auditHistory =>
        {
            auditHistory.ConfigureByConvention();
        });
 
        builder.Entity<EscalationMatrix>(EscalationMatrix =>
        {            
            EscalationMatrix.ConfigureByConvention();
        });
        builder.Entity<MeetingMinute>(MeetingMinute =>
        {            
            MeetingMinute.ConfigureByConvention();
        });
  
        builder.Entity<Project>(Project =>
        {
            Project.ConfigureByConvention();
        });
        builder.Entity<ProjectBudget>(ProjectBudget =>
        {
            ProjectBudget.ConfigureByConvention();
        });
        builder.Entity<ProjectResources>(ProjectResources =>
        {
            ProjectResources.ConfigureByConvention();
        });
        builder.Entity<RiskProfile>(RiskProfile =>
        {
            RiskProfile.ConfigureByConvention();
        });
        builder.Entity<Sprint>(Sprint =>
        {
            Sprint.ConfigureByConvention();
        });
        builder.Entity<PhaseMilestone>(PhaseMilestone =>
        {
            PhaseMilestone.ConfigureByConvention();
        });
        builder.Entity<ClientFeedback>(ClientFeedback =>
        {
            ClientFeedback.ConfigureByConvention();
        });
   
        builder.Entity<User>(ApplicationUser =>
        {
            ApplicationUser.ConfigureByConvention();
        });
       builder.Entity<UserRole>(userRole =>
        {
            userRole.ConfigureByConvention();
        });
       builder.Entity<Role>(role =>
        {
            role.ConfigureByConvention();
        });
       


    }
}
