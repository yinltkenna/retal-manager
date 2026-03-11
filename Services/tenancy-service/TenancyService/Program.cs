using System.Text;
using EventContracts.Authorization.PermissionsAuthorization;
using EventContracts.Authorization.Permissions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TenancyService.src.Application.Interfaces;
using TenancyService.src.Application.Mappings;
using TenancyService.src.Application.Services.Implementations;
using TenancyService.src.Application.Services.Interfaces;
using TenancyService.src.Application.Validator;
using TenancyService.src.Infrastructure.Data;
using TenancyService.src.Infrastructure.Repositories.Implementations;
using TenancyService.src.Infrastructure.Repositories.Interfaces;
using TenancyService.src.Web.Common.TemplateResponses;
using TenancyService.src.Web.Configurations;
using EventContracts.Authorization.Permissions.TenancyService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TenancyDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new InvalidOperationException("JwtSettings configuration is missing");

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

builder.Services.AddHttpContextAccessor();

builder.Services.AddPermissionAuthorization(
    TenantPermissions.VIEW,
    TenantPermissions.CREATE,
    TenantPermissions.UPDATE,
    TenantPermissions.DELETE,
    TenantContractPermissions.VIEW,
    TenantContractPermissions.CREATE,
    TenantContractPermissions.UPDATE,
    TenantContractPermissions.DELETE,
    TenantContractFilePermissions.VIEW,
    TenantContractFilePermissions.CREATE,
    TenantContractFilePermissions.DELETE,
    TenantContractMemberPermissions.VIEW,
    TenantContractMemberPermissions.CREATE,
    TenantContractMemberPermissions.DELETE,
    TenantIdentityDocumentsPermissions.VIEW,
    TenantIdentityDocumentsPermissions.CREATE,
    TenantIdentityDocumentsPermissions.UPDATE,
    TenantIdentityDocumentsPermissions.DELETE,
    TenantContractExtensionPermissions.VIEW,
    TenantContractExtensionPermissions.CREATE,
    TenantContractExtensionPermissions.UPDATE,
    TenantContractExtensionPermissions.DELETE,
    TenantDepositTransactionPermission.VIEW,
    TenantDepositTransactionPermission.CREATE,
    TenantDepositTransactionPermission.UPDATE,
    TenantDepositTransactionPermission.DELETE,
    TenantTerminationPermissions.VIEW,
    TenantTerminationPermissions.CREATE,
    TenantTerminationPermissions.UPDATE,
    TenantTerminationPermissions.DELETE
);

builder.Services.AddAutoMapper(typeof(TenancyMappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<CreateTenantRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddScoped<IPermissionChecker, PermissionChecker>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ITenantIdentityDocumentService, TenantIdentityDocumentService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IContractMemberService, ContractMemberService>();
builder.Services.AddScoped<IContractFileService, ContractFileService>();
builder.Services.AddScoped<IContractExtensionService, ContractExtensionService>();
builder.Services.AddScoped<IContractTerminationService, ContractTerminationService>();
builder.Services.AddScoped<IContractDepositTransactionService, ContractDepositTransactionService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new TenancyService.src.Web.Common.Helpers.VietnamDateTimeConverter());
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

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tenancy Service API", Version = "v1" });

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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGateway", policy =>
    {
        policy.WithOrigins("http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("AllowGateway");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            var gatewayUrl = "http://localhost:5000/tenancy";

            swagger.Servers = [
                new() { Url = gatewayUrl }
            ];
        });
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tenancy Service V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TenancyDbContext>();
    db.Database.Migrate();
}

app.Run();
