namespace SolarWatch.Client;

public class SolarWatchClient : ISolarWatchClient
{
    private static readonly HttpClient Client = new HttpClient();

    public HttpClient GetClient()
    {
        return Client;
    }
}