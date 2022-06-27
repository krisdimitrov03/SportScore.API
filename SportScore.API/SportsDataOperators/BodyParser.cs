using Newtonsoft.Json;

namespace SportScore.API.SportsDataOperators
{
    public class BodyParser
    {
        public async Task<T?> Parse<T>(Stream body)
            where T : class
        {
            string bodyAsString = string.Empty;

            using (var reader = new StreamReader(body))
            {
                bodyAsString = await reader.ReadToEndAsync();
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(bodyAsString);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
