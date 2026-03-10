using EventContracts.Authorization.Permissions.IdentityService;
using EventContracts.Authorization.PermissionsAuthorization;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityService.src.Application.Mapping;
using IdentityService.src.Application.Services.Implementations;
using IdentityService.src.Application.Services.Interfaces;
using IdentityService.src.Application.Validator;
using IdentityService.src.Infrastructure.Caching;
using IdentityService.src.Infrastructure.Clients;
using IdentityService.src.Infrastructure.Data;
using IdentityService.src.Infrastructure.Data.Seed;
using IdentityService.src.Infrastructure.Messaging.Publishers;
using IdentityService.src.Infrastructure.Repositories.Implementations;
using IdentityService.src.Infrastructure.Repositories.Interfaces;
using IdentityService.src.Web.Common.Helpers;
using IdentityService.src.Web.Common.TemplateResponses;
using IdentityService.src.Web.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// configuration and logging are already set

// add Entity Framework Core context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Register validators from the assembly containing the validators
builder.Services.AddValidatorsFromAssemblyContaining<RegisterTenantRequestValidator>();
// Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
// Controllers + json options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new VietnamDateTimeConverter());
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var validationErrors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Errors = e.Value.Errors.Select(er => er.ErrorMessage)
            }).ToList();

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "Validation failed",
            Data = validationErrors
        };

        return new BadRequestObjectResult(response);
    };
});
// configuration objects
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("Redis"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Authentication & Authorization
builder.Services.AddHttpContextAccessor();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

builder.Services.AddPermissionAuthorization(
    UserPermissions.VIEW,
    UserPermissions.CREATE,
    UserPermissions.UPDATE,
    UserPermissions.DELETE,
    UserPermissions.LOCK,
    UserPermissions.VIEW_LOGS
);

// infrastructure
builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>(sp =>
{
    var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMqSettings>>().Value;
    return new RabbitMqPublisher(opts);
});
builder.Services.AddSingleton<ICacheService, RedisCacheService>(sp =>
{
    var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RedisSettings>>().Value;
    return new RedisCacheService(opts);
});

// Repository
builder.Services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// infrastructure clients
builder.Services.AddHttpClient<ITenancyServiceClient, TenancyServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:TenancyService"]);
});
builder.Services.AddHttpClient<INotificationClient, NotificationClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:NotificationService"]);
});
// AutoMapper
builder.Services.AddAutoMapper(typeof(IdentityProfile));

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Property Service API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token in the text input below."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var validationErrors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Errors = e.Value.Errors.Select(er => er.ErrorMessage)
            })
            .ToList();

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "Validation failed",
            Data = validationErrors
        };

        return new BadRequestObjectResult(response);
    };
});

builder.Services.AddHttpClient();
var app = builder.Build();

// Add exception handling middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// For Migrate database on startup, ensure SQL Server is ready before applying migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await IdentitySeeder.SeedAsync(db, logger);
}
app.Run();
