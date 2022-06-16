#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace SportScore.API.DataTransferModels
{
    public class LineupDTO
    {
        public LineupDTO()
        {
            StartingEleven = new List<PlayerDTO>();
            Substitutes = new List<PlayerDTO>();
            Coaches = new List<string>();
            MissingPlayers = new List<PlayerDTO>();
        }

        public List<PlayerDTO> StartingEleven { get; set; }

        public List<PlayerDTO> Substitutes { get; set; }

        public List<string> Coaches { get; set; }

        public List<PlayerDTO> MissingPlayers { get; set; }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.