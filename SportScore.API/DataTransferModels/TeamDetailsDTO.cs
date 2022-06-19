namespace SportScore.API.DataTransferModels
{
    public class TeamDetailsDTO
    {
        public TeamDetailsDTO()
        {
            Matches = new List<FixtureDTO>();
            Players = new List<PlayerDTO>();
            Coaches = new List<string>();
        }

        public string Name { get; set; }

        public string Image { get; set; }

        public List<FixtureDTO> Matches { get; set; }

        public List<PlayerDTO> Players { get; set; }

        public List<string> Coaches { get; set; }
    }
}
