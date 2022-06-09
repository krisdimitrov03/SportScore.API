using Newtonsoft.Json;
using SportScore.API.Constants;
using SportScore.API.DataTransferModels;
using SportScore.API.SportsDataOperators.Contracts;
using System.Globalization;

namespace SportScore.API.SportsDataOperators.Services
{
    public class FootballService : IFootballService
    {
        private readonly ISportDataService sdService;

        public FootballService(ISportDataService _sdService)
        {
            sdService = _sdService;
        }

        public async Task<List<FixtureDTO>> GetFixturesByDate(string from, string to)
        {
            return await Fixtures(from, to);
        }

        public async Task<List<FixtureDTO>> GetLivescore()
        {
            return await Fixtures(met: MetConstants.Livescore);
        }

        public async Task<List<LeaguesByCountryDTO>> GetLeagues()
        {
            string rawData = await sdService.Get(UrlConstants.FootballUrl,
                new Dictionary<string, string>
                {
                    { ParamConstants.ApiKey, UrlConstants.APIKey },
                    { ParamConstants.Met, MetConstants.Leagues }
                });

            var data = JsonConvert.DeserializeObject<LeaguesRawDTO>(rawData);

            List<LeaguesByCountryDTO> result = new List<LeaguesByCountryDTO>();

            foreach (var league in data.Result)
            {
                var decisiveLeague = result.FirstOrDefault(e => e.Country == (string)league["country_name"]);

                LeagueDTO leagueObject = new LeagueDTO
                {
                    LeagueId = (string)league["league_key"],
                    Name = (string)league["league_name"]
                };

                if (decisiveLeague != null)
                {
                    decisiveLeague.Leagues.Add(leagueObject);
                }
                else
                {
                    result.Add(new LeaguesByCountryDTO
                    {
                        Country = (string)league["country_name"],
                        Leagues = new List<LeagueDTO> { leagueObject }
                    });
                }
            }

            return result
                .OrderBy(l => l.Country)
                .ToList();
        }

        private async Task<List<FixtureDTO>> Fixtures(string? from = null, string? to = null, string? met = null)
        {
            var parameters = new Dictionary<string, string>
                {
                    { ParamConstants.ApiKey, UrlConstants.APIKey },
                    { ParamConstants.Met, MetConstants.Fixtures }
                };

            if (met != null) parameters[ParamConstants.Met] = met;

            if (from != null) parameters.Add(ParamConstants.From, from);

            if (to != null) parameters.Add(ParamConstants.To, to);


            string rawData = await sdService.Get(UrlConstants.FootballUrl, parameters);

            var data = JsonConvert.DeserializeObject<FixtureRawDTO>(rawData);

            List<FixtureDTO> result = new List<FixtureDTO>();

            foreach (var match in data.Result)
            {
                var decisiveMatch = result.FirstOrDefault(e => e.League == (string)match["league_name"]);

                MatchDTO matchObject = new MatchDTO
                {
                    MatchId = (string)match["event_key"],
                    StartTime = (string)match["event_time"],
                    Time = (string)match["event_status"],
                    HomeTeam = (string)match["event_home_team"],
                    HomeGoals = ((string)match["event_final_result"]) != ""
                        ? ((string)match["event_final_result"]).Split(" - ")[0]
                        : "",
                    AwayTeam = (string)match["event_away_team"],
                    AwayGoals = ((string)match["event_final_result"]) != ""
                        ? ((string)match["event_final_result"]).Split(" - ")[0]
                        : ""
                };

                if (decisiveMatch != null)
                {
                    decisiveMatch.Matches.Add(matchObject);
                }
                else
                {
                    result.Add(new FixtureDTO
                    {
                        League = (string)match["league_name"],
                        Matches = new List<MatchDTO> { matchObject }
                    });
                }
            }

            return result;
        }
    }
}
