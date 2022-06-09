namespace SportScore.API.SportsDataOperators.Contracts
{
    public interface ISportDataService
    {
        Task<string> Get(string path, Dictionary<string, string> parameters);
    }
}
