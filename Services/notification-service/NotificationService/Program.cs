using NotificationService.src.Application.Services.Interfaces;
using NotificationService.src.Application.Services.Implementations;
using NotificationService.src.Application.EventHandlers;
using NotificationService.src.Infrastructure.Messaging;
using NotificationService.src.Infrastructure.Messaging.Interfaces;
using NotificationService.src.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// configuration objects
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));

// Add services to the container
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<UserRegisteredEventHandler>();
builder.Services.AddSingleton<IRabbitMqSubscriber, RabbitMqSubscriber>(sp =>
{
    var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMqSettings>>().Value;
    return new RabbitMqSubscriber(opts, sp.GetRequiredService<ILogger<RabbitMqSubscriber>>());
});
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Health check endpoint
app.MapGet("/health", () => Results.Ok("Notification Service is running"))
    .WithName("HealthCheck");

// Start RabbitMQ subscriber in background
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        logger.LogInformation("🚀 Starting Notification Service RabbitMQ subscription...");

        var subscriber = app.Services.GetRequiredService<IRabbitMqSubscriber>();
        var handler = app.Services.GetRequiredService<UserRegisteredEventHandler>();
        
        await subscriber.SubscribeToUserRegisteredEventsAsync(handler);

        logger.LogInformation("✅ RabbitMQ subscription started successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Failed to start RabbitMQ subscription");
    }
});

app.Run();

