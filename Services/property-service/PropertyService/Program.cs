using System.Text;
using EventContracts.Authorization.PermissionsAuthorization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PropertyService.src.Application.Interfaces;
using PropertyService.src.Application.Mappings;
using PropertyService.src.Application.Services.Implementations;
using PropertyService.src.Application.Services.Interfaces;
using PropertyService.src.Application.Validator;
using PropertyService.src.Infrastructure.Data;
using PropertyService.src.Infrastructure.Repositories.Implementations;
using PropertyService.src.Infrastructure.Repositories.Interfaces;
using PropertyService.src.Web.Configurations;
using PropertyService.src.Web.Common.TemplateResponses;
using EventContracts.Authorization.Permissions.PropertyService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PropertyDbContext>(options =>
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
    BranchPermissions.VIEW,
    BranchPermissions.CREATE,
    BranchPermissions.UPDATE,
    BranchPermissions.DELETE,
    RoomTypePermissions.VIEW,
    RoomTypePermissions.CREATE,
    RoomTypePermissions.UPDATE,
    RoomTypePermissions.DELETE,
    RoomPermissions.VIEW,
    RoomPermissions.CREATE,
    RoomPermissions.UPDATE,
    RoomPermissions.DELETE,
    AmenityPermissions.VIEW,
    AmenityPermissions.CREATE,
    AmenityPermissions.UPDATE,
    AmenityPermissions.DELETE,
    RoomFacilityPermissions.VIEW,
    RoomFacilityPermissions.CREATE,
    RoomFacilityPermissions.UPDATE,
    RoomFacilityPermissions.DELETE,
    ReservationPermissions.VIEW,
    ReservationPermissions.CREATE,
    ReservationPermissions.CANCEL,
    MaintenancePermissions.VIEW,
    MaintenancePermissions.CREATE,
    MaintenancePermissions.UPDATE,
    RoomHistoryPermissions.VIEW,
    RoomStatusLogPermissions.VIEW
);

builder.Services.AddAutoMapper(typeof(PropertyMappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<CreateBranchRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddScoped<IPermissionChecker, PermissionChecker>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomPriceService, RoomPriceService>();
builder.Services.AddScoped<IRoomImageService, RoomImageService>();
builder.Services.AddScoped<IAmenityService, AmenityService>();
builder.Services.AddScoped<IRoomTypeAmenityService, RoomTypeAmenityService>();
builder.Services.AddScoped<IRoomFacilityService, RoomFacilityService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IRoomHistoryService, RoomHistoryService>();
builder.Services.AddScoped<IRoomMaintenanceService, RoomMaintenanceService>();
builder.Services.AddScoped<IRoomStatusLogService, RoomStatusLogService>();

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

// Swagger
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
            var gatewayUrl = "http://localhost:5000/property";

            swagger.Servers = [
                new() { Url = gatewayUrl }
            ];
        });
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Property Service V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure database is migrated when the app starts
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PropertyDbContext>();
    db.Database.Migrate();
}

app.Run();
