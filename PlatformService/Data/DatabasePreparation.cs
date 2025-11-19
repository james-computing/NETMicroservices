using PlatformService.Models;

namespace PlatformService.Data
{
    public static class DatabasePreparation
    {
        public static void PopulateDatabase(IApplicationBuilder applicationBuilder)
        {
            using ( IServiceScope serviceScope = 
                applicationBuilder.ApplicationServices.CreateScope())
            {
                AppDbContext dbContext = 
                    serviceScope.ServiceProvider.GetService<AppDbContext>()!;
                SeedData(dbContext);
            }
        }

        private static void SeedData(AppDbContext dbContext)
        {
            if (!dbContext.Platforms.Any())
            {
                Console.WriteLine("--> Seeding data...");
                dbContext.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free"},
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" });
                dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        } 
    }
}
