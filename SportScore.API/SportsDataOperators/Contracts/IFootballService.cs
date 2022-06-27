using SportScore.API.DataTransferModels;

namespace SportScore.API.SportsDataOperators.Contracts
{
    public interface IFootballService
    {
        Task<List<FixtureDTO>> GetLivescore();

        Task<List<FixtureDTO>> GetFixturesByDate(string from, string to);

        Task<List<LeaguesByCountryDTO>> GetLeagues();

        Task<MatchDetailsDTO> GetMatchDetails(string id);

        Task<LeagueDetailsDTO> GetLeagueDetails(string id);

        Task<TeamDetailsDTO> GetTeamDetails(string id);

        Task<MatchDTO> GetMatchById(string id);
    }
}
