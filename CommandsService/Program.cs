using CommandsService.Data;
using CommandsService.DataServices.Async;
using CommandsService.DataServices.Sync.Grpc;
using CommandsService.EventProcessing;
using CommandsService.Profiles;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// In memory database
Console.WriteLine("--> Using InMem Db");
builder.Services.AddDbContext<AppDbContext>(opt =>
     opt.UseInMemoryDatabase("InMem"));

builder.Services.AddAutoMapper(cfg => { }, typeof(CommandsProfile));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();

// RabbitMQ
builder.Services.AddHostedService<MessageBusSubscriber>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// HttpsRedirection is failing for Kubernetes
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
