using Newtonsoft.Json;
using SportScore.API.Constants;
using SportScore.API.DataTransferModels;
using SportScore.API.SportsDataOperators.Contracts;
using System.Globalization;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8603 // Possible null reference return.

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
                    { ParamConstants.Met, MetConstants.Leagues }
                });

            var data = JsonConvert.DeserializeObject<RawDataDTO>(rawData);

            List<LeaguesByCountryDTO> result = new List<LeaguesByCountryDTO>();

            foreach (var league in data.Result)
            {
                var decisiveLeague = result.FirstOrDefault(e => e.Country == (string)league["country_name"]);

                LeagueDTO leagueObject = new LeagueDTO()
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
                    result.Add(new LeaguesByCountryDTO()
                    {
                        CountryLogo = (string)league["country_logo"],
                        Country = (string)league["country_name"],
                        Leagues = new List<LeagueDTO> { leagueObject }
                    });
                }
            }

            return result
                .OrderBy(l => l.Country)
                .ToList();
        }

        public async Task<MatchDetailsDTO> GetMatchDetails(string id)
        {
            string rawData = await sdService.Get(UrlConstants.FootballUrl,
                new Dictionary<string, string>
                {
                    { ParamConstants.Met, MetConstants.Fixtures },
                    { ParamConstants.MatchId, id }
                });

            var data = JsonConvert.DeserializeObject<RawDataDTO>(rawData).Result[0];

            string[] dateParts = ((string)data["event_date"]).Split("-");

            string formattedDate = $"{dateParts[2]}.{dateParts[1]}.{dateParts[0]}";

            var details = GetDetails(data);
            var stats = GetStats(data);
            var lineups = GetLineups(data);

            return new MatchDetailsDTO()
            {
                LeagueId = (string)data["league_key"],
                League = (string)data["league_name"],
                HomeTeam = (string)data["event_home_team"],
                HomeTeamLogo = (string)data["home_team_logo"],
                AwayTeam = (string)data["event_away_team"],
                AwayTeamLogo = (string)data["away_team_logo"],
                Result = (string)data["event_final_result"],
                Time = (string)data["event_status"],
                StartDateAndTime = $"{formattedDate}, {(string)data["event_time"]}",
                Country = (string)data["country_name"],
                Details = details,
                Statistics = stats,
                Lineups = lineups
            };
        }

        public async Task<LeagueDetailsDTO> GetLeagueDetails(string id)
        {
            return new LeagueDetailsDTO()
            {
                LeagueId = "sd1ds2dd2",
                LeagueName = "Liga 2",
                LeagueImage = "imageUrl",
                CountryId = "ad12EESD2",
                CountryName = "Angola"
            };
        }

        //-- Help methods --//

        private List<LineupDTO> GetLineups(Dictionary<string, object> data)
        {
            var lineupsData = JsonConvert.DeserializeObject<LineupsRawDTO>(data["lineups"].ToString());

            var homeLineup = GetSingleLineup(lineupsData.Home_Team);
            var awayLineup = GetSingleLineup(lineupsData.Away_Team);

            return new List<LineupDTO>() { homeLineup, awayLineup };
        }

        private LineupDTO GetSingleLineup(Dictionary<string, List<Dictionary<string, string>>> lineup)
        {
            LineupDTO result = new LineupDTO();

            foreach (var player in lineup["starting_lineups"])
            {
                result.StartingEleven.Add(new PlayerDTO()
                {
                    Name = player["player"],
                    Number = player["player_number"],
                    Position = player["player_position"]
                });
            }

            foreach (var player in lineup["substitutes"])
            {
                result.Substitutes.Add(new PlayerDTO()
                {
                    Name = player["player"],
                    Number = player["player_number"],
                    Position = player["player_position"]
                });
            }

            foreach (var coach in lineup["coaches"])
            {
                result.Coaches.Add(coach["coache"]);
            }

            foreach (var player in lineup["missing_players"])
            {
                result.MissingPlayers.Add(new PlayerDTO()
                {
                    Name = player["player"],
                    Number = player["player_number"],
                    Position = player["player_position"]
                });
            }

            return result;
        }

        private List<StatDTO> GetStats(Dictionary<string, object> data)
        {
            return JsonConvert.DeserializeObject<List<StatDTO>>(data["statistics"].ToString());
        }

        private List<MiniEventDTO> GetDetails(Dictionary<string, object> data)
        {
            var goalscorersData = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(data["goalscorers"].ToString());
            var cardsData = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(data["cards"].ToString());

            List<MiniEventDTO> details = new List<MiniEventDTO>();

            var goalscorers = goalscorersData.Select(e => new MiniEventDTO(
                e["time"].EndsWith("+") ? e["time"].Replace("+", "") : e["time"],
                "goal",
                e["home_scorer"] != "" ? "home" : "away",
                e["home_scorer"] != "" ? e["home_scorer"] : e["away_scorer"],
                e["score"]))
                .ToList();

            var cards = cardsData.Select(e => new MiniEventDTO(
                e["time"],
                e["card"],
                e["home_fault"] != "" ? "home" : "away",
                e["home_fault"] != "" ? e["home_fault"] : e["away_fault"]))
                .ToList();

            details.AddRange(goalscorers);
            details.AddRange(cards);

            return details
                .OrderBy(e => int.Parse(e.Time))
                .ToList();
        }

        private async Task<List<FixtureDTO>> Fixtures(string? from = null, string? to = null, string? met = null)
        {
            var parameters = new Dictionary<string, string>
                {
                    { ParamConstants.Met, MetConstants.Fixtures }
                };

            if (met != null) parameters[ParamConstants.Met] = met;

            if (from != null) parameters.Add(ParamConstants.From, from);

            if (to != null) parameters.Add(ParamConstants.To, to);


            string rawData = await sdService.Get(UrlConstants.FootballUrl, parameters);

            var data = JsonConvert.DeserializeObject<RawDataDTO>(rawData);

            List<FixtureDTO> result = new List<FixtureDTO>();

            foreach (var match in data.Result)
            {
                var decisiveMatch = result.FirstOrDefault(e => e.League == (string)match["league_name"]);

                MatchDTO matchObject = new MatchDTO()
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
                    result.Add(new FixtureDTO()
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

#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8604 // Possible null reference argument.