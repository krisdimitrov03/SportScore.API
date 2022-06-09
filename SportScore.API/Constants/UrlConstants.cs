namespace SportScore.API.Constants
{
    public class UrlConstants
    {
        public const string BaseUrl = "https://apiv2.allsportsapi.com";

        public const string FootballUrl = $"{BaseUrl}/football";

        public static string APIKey = Environment.GetEnvironmentVariable("API_KEY");
    }
}
