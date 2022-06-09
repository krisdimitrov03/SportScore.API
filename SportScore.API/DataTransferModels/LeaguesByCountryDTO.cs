namespace SportScore.API.DataTransferModels
{
    public class LeaguesByCountryDTO
    {
        public string Country { get; set; }

        public List<LeagueDTO> Leagues { get; set; }
    }
}
