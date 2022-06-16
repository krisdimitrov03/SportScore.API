namespace SportScore.API.DataTransferModels
{
    public class LeaguesByCountryDTO
    {
        public LeaguesByCountryDTO()
        {
            Leagues = new List<LeagueDTO>();
        }

        public string CountryLogo { get; set; }
        
        public string Country { get; set; }

        public List<LeagueDTO> Leagues { get; set; }
    }
}
