using CommandsService.DataServices.Sync.Grpc;
using CommandsService.Models;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            // Create a scope, so we can use GRPC
            using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
            {
                // Get the services
                IPlatformDataClient? grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                ICommandRepo? repo = serviceScope.ServiceProvider.GetService<ICommandRepo>();
                if (grpcClient == null || repo == null)
                {
                    Console.WriteLine("Failed to get service in PrepDb.PrepPopulation");
                    return;
                }

                // Use gRPC to get the platforms from the Platforms service
                IEnumerable<Platform>? platforms = grpcClient.ReturnAllPlatform();
                // If the gRPC succeeded, store the data in the Commands service database
                if(platforms != null)
                {
                    SeedData(repo,platforms);
                }
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new platforms...");
            foreach(Platform platform in platforms)
            {
                if(repo.ExternalPlatformExists(platform.ExternalId) == false)
                {
                    repo.CreatePlatform(platform);
                }
                repo.SaveChanges();
            }
        }
    }
}
