using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Profiles;
using PlatformService.SyncDataServices.Http;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

/*
string? connectionString = builder.Configuration.GetConnectionString("Default");
if(connectionString == null)
{
    Console.WriteLine("Failed to read connection string");
    return;
}
builder.Services.AddDbContext<AppDbContext>(
    (DbContextOptionsBuilder options) => options.UseSqlServer(connectionString)
    );
*/

Console.WriteLine("--> Using InMem Db");
builder.Services.AddDbContext<AppDbContext>(opt =>
     opt.UseInMemoryDatabase("InMem"));

builder.Services.AddScoped<IPlatformRepo,PlatformRepo>();
builder.Services.AddAutoMapper(cfg => { }, typeof(PlatformsProfile));
builder.Services.AddHttpClient<ICommandDataClient,HttpDataClientCommand>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DatabasePreparation.Population(app);

app.Run();
