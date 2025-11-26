using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PlatformService.Data;
using PlatformService.DataServices.AsyncDataServices;
using PlatformService.DataServices.SyncDataServices.Http;
using PlatformService.Profiles;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// SQL Server
string connectionStringLabel;
if (builder.Environment.IsDevelopment())
{
    connectionStringLabel = "DefaultDev";
}
else
{
    connectionStringLabel = "DefaultProd";
}

Console.WriteLine($"connectionStringLabel: {connectionStringLabel}");
string? connectionString = builder.Configuration.GetConnectionString(connectionStringLabel);
if (connectionString == null)
{
    Console.WriteLine("Failed to read connection string");
    return;
}
builder.Services.AddDbContext<AppDbContext>(
    (DbContextOptionsBuilder options) => options.UseSqlServer(connectionString)
    );

builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
builder.Services.AddAutoMapper(cfg => { }, typeof(PlatformsProfile));
builder.Services.AddHttpClient<ICommandDataClient, HttpDataClientCommand>();

// RabbitMQ
MessageBusClient messageBusClient = new MessageBusClient();
bool successfulConnection = await messageBusClient.InitializeRabbitMQ(builder.Environment, builder.Configuration);
if(successfulConnection == false)
{
    return;
}
builder.Services.AddSingleton<IMessageBusClient>(messageBusClient);

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

DatabasePreparation.PopulateDatabase(app);

app.Run();