using SportScore.API.Constants;
using SportScore.API.SportsDataOperators.Contracts;

namespace SportScore.API.SportsDataOperators.Services
{
    public class SportDataService : ISportDataService
    {
        public async Task<string> Get(
            string path,
            Dictionary<string, string>? prms = null)
        {

            HttpClient httpClient = new HttpClient();

            if(prms != null)
            {
                path += "?";

                prms.Add(ParamConstants.ApiKey, UrlConstants.APIKey);

                foreach (var param in prms)
                {
                    string paramUrl = $"{param.Key}={param.Value}&";
                    path += paramUrl;
                }

                path = path.Remove(path.Length - 1);
            }

            var response = await httpClient.GetAsync(path);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
