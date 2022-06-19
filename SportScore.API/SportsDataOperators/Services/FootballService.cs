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

            var details = await GetDetails(data);
            var stats = await GetStats(data);
            var lineups = await GetLineups(data);

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
            var leagueRaw = await sdService.Get(UrlConstants.FootballUrl,
                new Dictionary<string, string>()
                {
                    { ParamConstants.Met, MetConstants.Leagues }
                });

            var leagues = JsonConvert.DeserializeObject<RawDataDTO>(leagueRaw).Result;

            var leagueData = leagues.FirstOrDefault(e => (string)e["league_key"] == id);

            string name = (string)leagueData["league_name"];
            string image = (string)leagueData["league_logo"];
            string countryId = (string)leagueData["country_key"];
            string countryName = (string)leagueData["country_name"];

            var standingsRaw = await sdService.Get(UrlConstants.FootballUrl,
                new Dictionary<string, string>()
                {
                    { ParamConstants.Met, MetConstants.Standings },
                    { ParamConstants.LeagueId, id }
                });

            var standingsData = JsonConvert.DeserializeObject<AllStandingsRawDTO>(standingsRaw);

            List<TeamInStandingDTO> standings = await GetStandings(standingsData.Result.Total);

            var topscorersRaw = await sdService.Get(UrlConstants.FootballUrl,
                new Dictionary<string, string>()
                {
                    { ParamConstants.Met, MetConstants.Topscorers },
                    { ParamConstants.LeagueId, id }
                });

            var topscorersData = JsonConvert.DeserializeObject<RawDataDTO>(topscorersRaw);

            List<PlayerInTopScorersDTO> topscorers = await GetTopscorers(topscorersData.Result);

            return new LeagueDetailsDTO()
            {
                LeagueId = id,
                LeagueName = name,
                LeagueImage = image,
                CountryId = countryId,
                CountryName = countryName,
                Standings = standings,
                TopScorers = topscorers
            };
        }

        public async Task<TeamDetailsDTO> GetTeamDetails(string id)
        {
            string rawData = await sdService.Get(UrlConstants.FootballUrl,
                new Dictionary<string, string>()
                {
                    { ParamConstants.Met, MetConstants.Teams },
                    { ParamConstants.TeamId, id }
                });

            var teamDetails = JsonConvert.DeserializeObject<RawDataDTO>(rawData).Result[0];

            var players = await GetPlayers(JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(teamDetails["players"].ToString()));

            var date = DateTime.Now;

            var matches = await Fixtures(from: $"{date.Year}-{date.Month-3}-01", to: $"{date.Year}-{date.Month-3}-{DateTime.DaysInMonth(date.Year, date.Month)}", teamId: id);

            var coaches = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(teamDetails["coaches"].ToString())
                .Select(c => c["coach_name"])
                .ToList();

            return new TeamDetailsDTO()
            {
                Name = (string)teamDetails["team_name"],
                Image = (string)teamDetails["team_logo"],
                Matches = matches,
                Players = players,
                Coaches = coaches
            };
        }

        //-- Help methods --//

        private async Task<List<PlayerDTO>> GetPlayers(List<Dictionary<string, object>> data)
        {
            return data.Select(p => new PlayerDTO()
            {
                Name = (string)p["player_name"],
                Image = (string)p["player_image"],
                Position = (string)p["player_type"]
            }).ToList();
        }

        private async Task<List<TeamInStandingDTO>> GetStandings(List<Dictionary<string, string>> data)
        {
            return data.Select(t => new TeamInStandingDTO()
            {
                Number = int.Parse(t["standing_place"]),
                Name = t["standing_team"],
                GamesPlayed = t["standing_P"] == null ? 0 : int.Parse(t["standing_P"]),
                Wins = t["standing_W"] == null ? 0 : int.Parse(t["standing_W"]),
                Draws = t["standing_D"] == null ? 0 : int.Parse(t["standing_D"]),
                Losts = t["standing_L"] == null ? 0 : int.Parse(t["standing_L"]),
                Points = t["standing_PTS"] == null ? 0 : int.Parse(t["standing_PTS"])
            }).ToList();
        }

        private async Task<List<PlayerInTopScorersDTO>> GetTopscorers(List<Dictionary<string, object>> data)
        {
            return data.Select(p => new PlayerInTopScorersDTO()
            {
                Number = int.Parse((string)p["player_place"]),
                Name = (string)p["player_name"],
                Team = (string)p["team_name"],
                Goals = p["goals"] == null ? 0 : int.Parse((string)p["goals"]),
                Assists = p["assists"] == null ? 0 : int.Parse((string)p["assists"]),
                PenaltyGoals = p["penalty_goals"] == null ? 0 : int.Parse((string)p["penalty_goals"])
            }).ToList();
        }

        private async Task<List<LineupDTO>> GetLineups(Dictionary<string, object> data)
        {
            var lineupsData = JsonConvert.DeserializeObject<LineupsRawDTO>(data["lineups"].ToString());

            var homeLineup = await GetSingleLineup(lineupsData.Home_Team);
            var awayLineup = await GetSingleLineup(lineupsData.Away_Team);

            return new List<LineupDTO>() { homeLineup, awayLineup };
        }

        private async Task<LineupDTO> GetSingleLineup(Dictionary<string, List<Dictionary<string, string>>> lineup)
        {
            LineupDTO result = new LineupDTO();

            foreach (var player in lineup["starting_lineups"])
            {
                result.StartingEleven.Add(new PlayerInLineupDTO()
                {
                    Name = player["player"],
                    Number = player["player_number"],
                    Position = player["player_position"]
                });
            }

            foreach (var player in lineup["substitutes"])
            {
                result.Substitutes.Add(new PlayerInLineupDTO()
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
                result.MissingPlayers.Add(new PlayerInLineupDTO()
                {
                    Name = player["player"],
                    Number = player["player_number"],
                    Position = player["player_position"]
                });
            }

            return result;
        }

        private async Task<List<StatDTO>> GetStats(Dictionary<string, object> data)
        {
            return JsonConvert.DeserializeObject<List<StatDTO>>(data["statistics"].ToString());
        }

        private async Task<List<MiniEventDTO>> GetDetails(Dictionary<string, object> data)
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

        private async Task<List<FixtureDTO>> Fixtures(string? from = null, string? to = null, string? met = null, string? teamId = null)
        {
            var parameters = new Dictionary<string, string>
                {
                    { ParamConstants.Met, MetConstants.Fixtures }
                };

            if (met != null) parameters[ParamConstants.Met] = met;

            if (from != null) parameters.Add(ParamConstants.From, from);

            if (to != null) parameters.Add(ParamConstants.To, to);

            if (teamId != null) parameters.Add(ParamConstants.TeamId, teamId);


            string rawData = await sdService.Get(UrlConstants.FootballUrl, parameters);

            var data = JsonConvert.DeserializeObject<RawDataDTO>(rawData);

            List<FixtureDTO> result = new List<FixtureDTO>();

            if(data.Result != null)
            {
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
            }

            return result;
        }
    }
}

#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8604 // Possible null reference argument.