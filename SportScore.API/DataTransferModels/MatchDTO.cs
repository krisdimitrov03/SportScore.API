#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SportScore.API.DataTransferModels
{
    public class MatchDTO
    {
        public string MatchId { get; set; }

        public string StartTime { get; set; }

        public string Time { get; set; }

        public string HomeTeam { get; set; }

        public string HomeGoals { get; set; }

        public string AwayTeam { get; set; }

        public string AwayGoals { get; set; }
    }
}
