using SportScore.API.DataTransferModels;

namespace SportScore.API.DataTransferModels
{
    public class FixtureDTO
    {
        public FixtureDTO()
        {
            Matches = new List<MatchDTO>();
        }

        public string Id { get; set; }

        public string Image { get; set; }

        public string? League { get; set; }

        public List<MatchDTO> Matches { get; set; }
    }
}
