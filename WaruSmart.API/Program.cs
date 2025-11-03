using WaruSmart.API.Profiles.Domain.Model.Entities;
using WaruSmart.API.Profiles.Domain.Repositories;
using WaruSmart.API.Profiles.Domain.Services;
using WaruSmart.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using WaruSmart.API.Profiles.Application.Internal.CommandServices;
using WaruSmart.API.Profiles.Application.Internal.QueryServices;
using WaruSmart.API.Profiles.Interfaces.ACL;
using WaruSmart.API.Profiles.Interfaces.ACL.Services;
using WaruSmart.API.Profiles.Infrastructure.Persistence.EFC;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC;
using WaruSmart.API.Forum.Application.CommandServices;
using WaruSmart.API.Forum.Application.QueryService;
using WaruSmart.API.Forum.Domain.Repositories;
using WaruSmart.API.Forum.Domain.Services;
using WaruSmart.API.Forum.Infrastructure.Persistence.EFC.Repositories;
using WaruSmart.API.IAM.Application.Internal.CommandServices;
using WaruSmart.API.IAM.Application.Internal.OutboundServices;
using WaruSmart.API.IAM.Application.Internal.QueryServices;
using WaruSmart.API.IAM.Domain.Repositories;
using WaruSmart.API.IAM.Domain.Services;
using WaruSmart.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using WaruSmart.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using WaruSmart.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using WaruSmart.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using WaruSmart.API.IAM.Infrastructure.Tokens.JWT.Services;
using WaruSmart.API.IAM.Interfaces.ACL;
using WaruSmart.API.IAM.Interfaces.ACL.Services;
using WaruSmart.API.Shared.Domain.Repositories;
using  WaruSmart.API.Shared.Interfaces.ASP.Configuration;
using  WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using  WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WaruSmart.API.Crops.Application.CommandServices;
using WaruSmart.API.Crops.Application.EventServices;
using WaruSmart.API.Crops.Application.QueryServices;
using WaruSmart.API.Crops.Domain.Repositories;
using WaruSmart.API.Crops.Domain.Services;
using WaruSmart.API.Crops.Infrastructure.Persistence.EFC.Repositories;
using WaruSmart.API.Resources.Application.InboundServices;
using WaruSmart.API.Resources.Domain.Repositories;
using WaruSmart.API.Resources.Domain.Services;
using WaruSmart.API.Resources.Infrastructure.Persistence.EFC;
using WaruSmart.API.IAM.Interfaces.REST.Transform;
using WaruSmart.API.Shared.Infrastructure.Persistence.EFC.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new KebabCaseRouteNamingConvention());
});


// Add Database Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Database Context and Logging Level
builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (connectionString != null)
            if (builder.Environment.IsDevelopment())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            else if (builder.Environment.IsProduction())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Error)
                    .EnableDetailedErrors();    
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "WaruSmart.CODEX.API",
                Version = "v1",
                Description = "WaruSmart Codex Platform API",
                TermsOfService = new Uri("https://github.com/upc-pre-202510-1asi0572-2947-codex"),
                Contact = new OpenApiContact
                {
                    Name = "CODEX development team",
                    Email = ""
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            } 
        });
    });


builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedAllPolicy",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Shared Bounded Context Dependency Injections
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Sowings Bounded Context Dependency Injections

builder.Services.AddScoped<ISowingRepository, SowingRepository>();
builder.Services.AddScoped<ISowingCommandService, SowingCommandService>();
builder.Services.AddScoped<ISowingQueryService, SowingQueryService>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceCommandService, DeviceCommandService>();
builder.Services.AddScoped<IDeviceQueryService, DeviceQueryService>();

//Controls Bounded Context Dependency Injections

builder.Services.AddScoped<IControlRepository, ControlRepository>();
builder.Services.AddScoped<IControlCommandService, ControlCommandService>();
builder.Services.AddScoped<IControlQueryService, ControlQueryService>();

//Crops Bounded Context Dependency Injections

builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<ICropCommandService, CropCommandService>();
builder.Services.AddScoped<ICropQueryService, CropQueryService>();

/*builder.Services.AddScoped<IDiseaseRepository, DiseaseRepository>();
builder.Services.AddScoped<IDiseaseCommandService, DiseaseCommandService>();
builder.Services.AddScoped<IDiseaseQueryService, DiseaseQueryService>();*/

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCommandService, ProductCommandService>();
builder.Services.AddScoped<IProductQueryService, ProductQueryService>();

/*builder.Services.AddScoped<IPestRepository, PestRepository>();
builder.Services.AddScoped<IPestCommandService, PestCommandService>();
builder.Services.AddScoped<IPestQueryService, PestQueryService>();*/

/*builder.Services.AddScoped<ICareRepository, CareRepository>();
builder.Services.AddScoped<ICareCommandService, CareCommandService>();
builder.Services.AddScoped<ICareQueryService, CareQueryService>();*/

//Forum Bounded Context Dependency Injections
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryCommandService, CategoryCommandService>();
builder.Services.AddScoped<ICategoryQueryService, CategoryQueryService>();

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IQuestionCommandService, QuestionCommandService>();
builder.Services.AddScoped<IQuestionQueryService, QuestionQueryService>();

builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IAnswerCommandService, AnswerCommandService>();
builder.Services.AddScoped<IAnswerQueryService, AnswerQueryService>();

// IAM Bounded Context Injection Configuration
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

builder.Services.AddScoped<IControlRepository, ControlRepository>();
builder.Services.AddScoped<IControlCommandService, ControlCommandService>();
builder.Services.AddScoped<IControlQueryService, ControlQueryService>();

// Profiles Bounded Context Dependency Injections
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

// Add Subscription Services to IAM Context
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

// Analytics Bounded Context Dependency Injections
builder.Services.AddScoped<WaruSmart.API.Analytics.Domain.Repositories.IAnalyticsRepository, WaruSmart.API.Analytics.Infrastructure.Persistence.AnalyticsRepository>();
builder.Services.AddScoped<WaruSmart.API.Analytics.Application.QueryServices.IAnalyticsQueryService, WaruSmart.API.Analytics.Application.QueryServices.AnalyticsQueryService>();
builder.Services.AddHostedService<WaruSmart.API.Resources.Infrastructure.Persistence.EFC.FogSyncBackgroundService>();



// Add Subscription Assemblers
builder.Services.AddScoped<SubscriptionResourceFromEntityAssembler>();
builder.Services.AddScoped<CreateSubscriptionCommandFromResourceAssembler>();
builder.Services.AddScoped<UpdateSubscriptionCommandFromResourceAssembler>();

builder.Services.AddScoped<IProductsBySowingRepository, ProductsBySowingRepository>();

builder.Services.AddScoped<IIoTDataRepository, IoTDataRepository>();
builder.Services.AddScoped<IFogSyncService, FogSyncService>();

builder.Services.AddHttpClient<IDeviceEventService, DeviceEventService>();

// Audit Trail Services
builder.Services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
builder.Services.AddScoped<IAuditTrailCommandService, AuditTrailCommandService>();

var app = builder.Build();


// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    if (app.Environment.IsDevelopment())
    {
        // In development, drop and recreate the database
        context.Database.EnsureCreated();
    }
    else
    {
        // In production, apply migrations
        context.Database.Migrate();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS Policy
app.UseCors("AllowedAllPolicy");

app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();