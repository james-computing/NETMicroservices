namespace HttpRequests
{
    internal class Program
    {
        public async static Task Main()
        {
            await PlatformsServiceRequests.PostAsync();
            //await NodePortRequests.GetAsync();
            //await CommandsServiceRequests.GetAsync();
            //await CommandsServiceRequests.PostAsync();
        }
    }
}