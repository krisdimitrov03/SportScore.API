namespace SportScore.API.DataTransferModels
{
    public class LeaguesByCountryDTO
    {
        public LeaguesByCountryDTO()
        {
            Leagues = new List<LeagueDTO>();
        }

        public string CountryId { get; set; }

        public string CountryLogo { get; set; }
        
        public string Country { get; set; }

        public List<LeagueDTO> Leagues { get; set; }
    }
}
