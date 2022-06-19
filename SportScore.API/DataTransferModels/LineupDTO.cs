#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SportScore.API.DataTransferModels
{
    public class LineupDTO
    {
        public LineupDTO()
        {
            StartingEleven = new List<PlayerInLineupDTO>();
            Substitutes = new List<PlayerInLineupDTO>();
            Coaches = new List<string>();
            MissingPlayers = new List<PlayerInLineupDTO>();
        }

        public List<PlayerInLineupDTO> StartingEleven { get; set; }

        public List<PlayerInLineupDTO> Substitutes { get; set; }

        public List<string> Coaches { get; set; }

        public List<PlayerInLineupDTO> MissingPlayers { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.