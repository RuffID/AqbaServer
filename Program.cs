using AqbaServer.Authorization;
using AqbaServer.Helper;
using AqbaServer.Interfaces.Authorization;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Interfaces.Service;
using AqbaServer.Repository.Authorization;
using AqbaServer.Repository.OkdeskEntities;
using AqbaServer.Repository.OkdeskPerformance;
using AqbaServer.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
    builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
    builder.Services.AddScoped<IKindRepository, KindRepository>();
    builder.Services.AddScoped<IKindParameterRepository, KindParameterRepository>();
    builder.Services.AddScoped<IKindParamRepository, KindParamRepository>();
    builder.Services.AddScoped<IModelRepository, ModelRepository>();
    builder.Services.AddScoped<IEquipmentParameterRepository, EquipmentParameterRepository>();
    builder.Services.AddScoped<IMaintenanceEntityRepository, MaintenanceEntityRepository>();
    builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
    builder.Services.AddScoped<IEmployeeGroupsRepository, EmployeeGroupsRepository>();
    builder.Services.AddScoped<IEmployeeRolesRepository, EmployeeRolesRepository>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IGroupRepository, GroupRepository>();
    builder.Services.AddScoped<IEmployeePerformanceRepository, EmployeePerformanceRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
    builder.Services.AddScoped<IManageImage, ManageImage>();
    builder.Services.AddScoped<IIssuePriorityRepository, IssuePriorityRepository>();
    builder.Services.AddScoped<IIssueStatusRepository, IssueStatusRepository>();
    builder.Services.AddScoped<IIssueTypeRepository, IssueTypeRepository>();
    builder.Services.AddScoped<IIssueRepository, IssueRepository>();
    builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
    
    builder.Services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // указывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = AuthOptions.ISSUER,
                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = AuthOptions.AUDIENCE,
                // будет ли валидироваться время существования
                ValidateLifetime = true,
                // установка ключа безопасности
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true,
            };
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "AqbaServerAPI", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    });

    builder.Configuration.AddJsonFile(Config.Path);

    builder.Services.AddHostedService<CheckLogFilesService>();

    builder.Services.AddHostedService<LoginService>();
    builder.Services.AddHostedService<UpdateDirectoriesService>();
    builder.Services.AddHostedService<ThirtyMinutesReportService>();
    builder.Services.AddHostedService<OneDayReportService>();
    builder.Services.AddHostedService<WeekReportService>();

    var app = builder.Build();
    Config.LoadConfig(app.Configuration);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // global cors policy for authorization
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.Run();
}
catch (Exception ex)
{
    WriteLog.Error(ex.ToString());
}
finally
{
    WriteLog.Info($"Shutdown service");
}