using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Promact.CustomerSuccess.Platform.Data;
using Promact.CustomerSuccess.Platform.Localization;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict;
using Volo.Abp.Security.Claims;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
<<<<<<< HEAD
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Promact.CustomerSuccess.Platform.Constants;
using Promact.CustomerSuccess.Platform.Services.Uttils;
using Promact.CustomerSuccess.Platform.Entities;
using Microsoft.AspNetCore.Identity;
=======
using Volo.Abp.AspNetCore.Authentication.OpenIdConnect;
using Autofac.Core;
>>>>>>> 35a62e36a313374652b1e47d6a37ad35f546a32a

namespace Promact.CustomerSuccess.Platform;

[DependsOn(
    // ABP Framework packages
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAutofacModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),

    // Account module packages
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountWebOpenIddictModule),

    // Identity module packages
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),

    // Audit logging module packages
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),

    // Permission Management module packages
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),

    // Tenant Management module packages
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),

    // Feature Management module packages
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementHttpApiModule),

    // Setting Management module packages
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementHttpApiModule)
)]
[DependsOn(typeof(AbpEmailingModule))]
public class PlatformModule : AbpModule
{
    /* Single point to enable/disable multi-tenancy */
    private const bool IsMultiTenant = true;

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        context.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        context.Services.AddScoped<IEmailService, EmailService>();
        context.Services.AddHttpClient();
        context.Services.AddTransient<UserManager<ApplicationUser>>();

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(PlatformResource)
            );
        });

        context.Services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        PreConfigure<OpenIddictBuilder>(builder =>
        {
            builder.AddValidation(options =>
            {
                options.AddAudiences("Platform");
                options.UseLocalServer();
                options.UseAspNetCore();
            });
        });

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                //serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "a2a5a8af-14c6-4374-a8f3-908165814c47");
                serverBuilder.AddEncryptionCertificate(
                    GetEncryptionCertificate(hostingEnvironment, context.Services.GetConfiguration()));
                serverBuilder.AddSigningCertificate(
                        GetSigningCertificate(hostingEnvironment, context.Services.GetConfiguration()));
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();
        context.Services.AddScoped<IUttilService, UttillService>();


        if (hostingEnvironment.IsDevelopment())
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
        }

<<<<<<< HEAD
        ConfigureAuthentication(context);


        context.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

         .AddJwtBearer(options =>
         {

             options.RequireHttpsMetadata = false;
             options.SaveToken = true;
             options.TokenValidationParameters = new TokenValidationParameters
             {

                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["jwt:Key"])),
                 ValidateIssuer = false,
                 ValidateAudience = false
             };
         });



        ConfigureAuthorization(context);
=======
        ConfigureAuthentication(context, configuration);
>>>>>>> 35a62e36a313374652b1e47d6a37ad35f546a32a
        ConfigureBundles();
        ConfigureMultiTenancy();
        ConfigureUrls(configuration);
        ConfigureAutoMapper(context);
        ConfigureSwagger(context.Services, configuration);
        ConfigureAutoApiControllers();
        ConfigureVirtualFiles(hostingEnvironment);
        ConfigureLocalization();
        ConfigureCors(context, configuration);
        ConfigureDataProtection(context);
        ConfigureEfCore(context);
    }

<<<<<<< HEAD

    private void ConfigureAuthentication(ServiceConfigurationContext context)
=======
    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
>>>>>>> 35a62e36a313374652b1e47d6a37ad35f546a32a
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = "KeycloakDemo";
            })
            .AddAbpOpenIdConnect(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];

                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.UsePkce = true;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.ClientId = configuration["AuthServer:ClientId"];

                //options.Scope.Add("role");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("phone");

                /*
                 * What I've done here will be built-in with ABP v5.3.0 (then we can delete the following code)
                 * https://github.com/abpframework/abp/pull/12085
                 */

                if (AbpClaimTypes.Name != "given_name")
                {
                    options.ClaimActions.MapJsonKey(AbpClaimTypes.Name, "given_name");
                    options.ClaimActions.DeleteClaim("given_name");
                    options.ClaimActions.RemoveDuplicate(AbpClaimTypes.Name);
                }

                if (AbpClaimTypes.SurName != "family_name")
                {
                    options.ClaimActions.MapJsonKey(AbpClaimTypes.SurName, "family_name");
                    options.ClaimActions.DeleteClaim("family_name");
                    options.ClaimActions.RemoveDuplicate(AbpClaimTypes.SurName);
                }
            });
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }


    private void ConfigureAuthorization(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorization(options =>
        {
            // Project policies
            options.AddPolicy(PolicyName.ProjectCreatePolicy, policy =>
            {
                policy.RequireRole("admin", "auditor");
            });
            options.AddPolicy(PolicyName.ProjectUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "auditor");
            });
            options.AddPolicy(PolicyName.ProjectDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "auditor");
            });


            // User policies
            options.AddPolicy(PolicyName.UserGetPolicy, policy =>
            {
                policy.RequireRole("admin");
            });
            options.AddPolicy(PolicyName.UserCreatePolicy, policy =>
            {
                policy.RequireRole("admin");
            });
            options.AddPolicy(PolicyName.UserUpdatePolicy, policy =>
            {
                policy.RequireRole("admin");
            });
            options.AddPolicy(PolicyName.UserDeletePolicy, policy =>
            {
                policy.RequireRole("admin");
            });

            // Role policies
            options.AddPolicy(PolicyName.RoleGetPolicy, policy =>
            {
                policy.RequireRole("admin");
            });
            options.AddPolicy(PolicyName.RoleCreatePolicy, policy =>
            {
                policy.RequireRole("admin");
            });
            options.AddPolicy(PolicyName.RoleUpdatePolicy, policy =>
            {
                policy.RequireRole("admin");
            });
            options.AddPolicy(PolicyName.RoleDeletePolicy, policy =>
            {
                policy.RequireRole("admin");
            });
            options.AddPolicy(PolicyName.AssignRolePolicy, policy =>
            {
                policy.RequireRole("admin","manager");
            });


            // Policies for creating individual resources
            options.AddPolicy(PolicyName.ProjectBudgetCreatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            options.AddPolicy(PolicyName.ProjectBudgetUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });
            options.AddPolicy(PolicyName.ProjectBudgetDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            //Sprint Policies

            options.AddPolicy(PolicyName.SprintCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "manager");
        });
            options.AddPolicy(PolicyName.SprintUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });
            options.AddPolicy(PolicyName.SprintDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            //Version History Policy


            options.AddPolicy(PolicyName.VersionHistoryCreatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            options.AddPolicy(PolicyName.VersionHistoryUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            options.AddPolicy(PolicyName.VersionHistoryDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            //Audit History Policy

            options.AddPolicy(PolicyName.AuditHistoryCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "manager");
        });


            options.AddPolicy(PolicyName.AuditHistoryUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            options.AddPolicy(PolicyName.AuditHistoryDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            //Client Feedback Delete Plolicy

            options.AddPolicy(PolicyName.ClientFeedbackCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "manager");
        });


            options.AddPolicy(PolicyName.ClientFeedbackUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });
            options.AddPolicy(PolicyName.ClientFeedbackDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });



            //Minite meeting policy
            options.AddPolicy(PolicyName.MinuteMeetingCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "auditor");
        });

            options.AddPolicy(PolicyName.MinuteMeetingUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            options.AddPolicy(PolicyName.MinuteMeetingDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            //Policy for Risk profile

            options.AddPolicy(PolicyName.RiskProfileCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "manager");
        });
            options.AddPolicy(PolicyName.RiskProfileUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            options.AddPolicy(PolicyName.RiskProfileDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            //Resource Policy


            options.AddPolicy(PolicyName.ResourceCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "manager");
        });
            options.AddPolicy(PolicyName.ResourceUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });
            options.AddPolicy(PolicyName.ResourceDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            //Stakeholder Policy

            options.AddPolicy(PolicyName.StakeholderCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "manager");
        });
            options.AddPolicy(PolicyName.StakeholderUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            options.AddPolicy(PolicyName.StakeholderDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            //EscalationMatrix Policy

            options.AddPolicy(PolicyName.EscalationMatrixCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "manager");
        });

            options.AddPolicy(PolicyName.EscalationMatrixUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });


            options.AddPolicy(PolicyName.EscalationMatrixDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            //ApprovedTeam Policy
            options.AddPolicy(PolicyName.ApproveTeamCreatePolicy, policy =>
        {
            policy.RequireRole("admin", "manager");
        });
            options.AddPolicy(PolicyName.ApproveTeamUpdatePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });

            options.AddPolicy(PolicyName.ApproveTeamDeletePolicy, policy =>
            {
                policy.RequireRole("admin", "manager");
            });
        });
    }


    private X509Certificate2 GetSigningCertificate(IWebHostEnvironment hostingEnv,
                            IConfiguration configuration)
    {
        var fileName = $"cert-signing.pfx";
        var passPhrase = configuration["MyAppCertificate:X590:PassPhrase"];
        var file = Path.Combine(hostingEnv.ContentRootPath, fileName);
        if (File.Exists(file))
        {
            var created = File.GetCreationTime(file);
            var days = (DateTime.Now - created).TotalDays;
            if (days > 180)
                File.Delete(file);
            else
                return new X509Certificate2(file, passPhrase,
                             X509KeyStorageFlags.MachineKeySet);
        }
        // file doesn't exist or was deleted because it expired
        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
        var request = new CertificateRequest(subject, algorithm,
                            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509KeyUsageExtension(
                            X509KeyUsageFlags.DigitalSignature, critical: true));
        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow,
                            DateTimeOffset.UtcNow.AddYears(2));
        File.WriteAllBytes(file, certificate.Export(X509ContentType.Pfx, string.Empty));
        return new X509Certificate2(file, passPhrase,
                            X509KeyStorageFlags.MachineKeySet);
    }

    private X509Certificate2 GetEncryptionCertificate(IWebHostEnvironment hostingEnv,
                                 IConfiguration configuration)
    {
        var fileName = $"cert-encryption.pfx";
        var passPhrase = configuration["MyAppCertificate:X590:PassPhrase"];
        var file = Path.Combine(hostingEnv.ContentRootPath, fileName);
        if (File.Exists(file))
        {
            var created = File.GetCreationTime(file);
            var days = (DateTime.Now - created).TotalDays;
            if (days > 180)
                File.Delete(file);
            else
                return new X509Certificate2(file, passPhrase,
                                X509KeyStorageFlags.MachineKeySet);
        }

        // file doesn't exist or was deleted because it expired
        using var algorithm = RSA.Create(keySizeInBits: 2048);
        var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
        var request = new CertificateRequest(subject, algorithm,
                            HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509KeyUsageExtension(
                            X509KeyUsageFlags.KeyEncipherment, critical: true));
        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow,
                            DateTimeOffset.UtcNow.AddYears(2));
        File.WriteAllBytes(file, certificate.Export(X509ContentType.Pfx, string.Empty));
        return new X509Certificate2(file, passPhrase, X509KeyStorageFlags.MachineKeySet);
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle => { bundle.AddFiles("/global-styles.css"); }
            );
        });
    }

    private void ConfigureMultiTenancy()
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = IsMultiTenant;
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());

            options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
            options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
        });
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<PlatformResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Platform");

            options.DefaultResourceType = typeof(PlatformResource);

            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            options.Languages.Add(new LanguageInfo("ar", "ar", "العربية"));
            options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
            options.Languages.Add(new LanguageInfo("en-GB", "en-GB", "English (UK)"));
            options.Languages.Add(new LanguageInfo("hu", "hu", "Magyar"));
            options.Languages.Add(new LanguageInfo("fi", "fi", "Finnish"));
            options.Languages.Add(new LanguageInfo("fr", "fr", "Français"));
            options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
            options.Languages.Add(new LanguageInfo("is", "is", "Icelandic", "is"));
            options.Languages.Add(new LanguageInfo("it", "it", "Italiano", "it"));
            options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
            options.Languages.Add(new LanguageInfo("ro-RO", "ro-RO", "Română"));
            options.Languages.Add(new LanguageInfo("ru", "ru", "Русский"));
            options.Languages.Add(new LanguageInfo("sk", "sk", "Slovak"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsch", "de"));
            options.Languages.Add(new LanguageInfo("es", "es", "Español"));
            options.Languages.Add(new LanguageInfo("el", "el", "Ελληνικά"));
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Platform", typeof(PlatformResource));
        });
    }

    private void ConfigureVirtualFiles(IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PlatformModule>();
            if (hostingEnvironment.IsDevelopment())
            {
                /* Using physical files in development, so we don't need to recompile on changes */
                options.FileSets.ReplaceEmbeddedByPhysical<PlatformModule>(hostingEnvironment.ContentRootPath);
            }
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(PlatformModule).Assembly);
        });
    }

    private void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAbpSwaggerGenWithOAuth(
            configuration["AuthServer:Authority"]!,
            new Dictionary<string, string>
            {
            {"Platform", "Platform API"}
            },
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Platform API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);

                // Define security scheme for bearer token
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                // Add JWT bearer token authentication requirement
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
                });
            });
    }

    private void ConfigureAutoMapper(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<PlatformModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            /* Uncomment `validate: true` if you want to enable the Configuration Validation feature.
             * See AutoMapper's documentation to learn what it is:
             * https://docs.automapper.org/en/stable/Configuration-validation.html
             */
            options.AddMaps<PlatformModule>(/* validate: true */);
        });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddCors(options =>
        {

            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray() ?? Array.Empty<string>()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureDataProtection(ServiceConfigurationContext context)
    {
        context.Services.AddDataProtection().SetApplicationName("Platform");
    }

    private void ConfigureEfCore(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PlatformDbContext>(options =>
        {
            /* You can remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots
             * Documentation: https://docs.abp.io/en/abp/latest/Entity-Framework-Core#add-default-repositories
             */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(configurationContext =>
            {
                configurationContext.UseNpgsql();
            });
        });


    }




    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("AllowOrigin");
        }

        app.UseAbpRequestLocalization();


        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<PlatformDbContext>();
                    db.Database.Migrate();
                    Log.Information("Mirgated Successfully");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }

        }

        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (IsMultiTenant)
        {
            app.UseMultiTenancy();
        }


        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Platform API");

            var configuration = context.GetConfiguration();
            options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
            options.OAuthScopes("Platform");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
