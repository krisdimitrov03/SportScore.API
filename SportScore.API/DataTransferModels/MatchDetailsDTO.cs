#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SportScore.API.DataTransferModels
{
    public class MatchDetailsDTO
    {
        public string Country { get; set; }

        public string League { get; set; }
        public string LeagueId { get; set; }

        public string HomeTeam { get; set; }
        public string HomeTeamLogo { get; set; }

        public string AwayTeam { get; set; }
        public string AwayTeamLogo { get; set; }

        public string Result { get; set; }

        public string Time { get; set; }

        public string StartDateAndTime { get; set; }

        public List<MiniEventDTO> Details { get; set; }

        public List<StatDTO> Statistics { get; set; }

        public List<LineupDTO> Lineups { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.