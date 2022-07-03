namespace SportScore.API.DataTransferModels
{
    public class LeagueDetailsDTO
    {
        public LeagueDetailsDTO()
        {
            Standings = new List<TeamInStandingDTO>();
            TopScorers = new List<PlayerInTopScorersDTO>();
        }

        public string LeagueId { get; set; }

        public string LeagueName { get; set; }

        public string LeagueImage { get; set; }

        public string CountryId { get; set; }

        public string CountryName { get; set; }

        public string CountryImage { get; set; }

        public List<TeamInStandingDTO> Standings { get; set; }

        public List<PlayerInTopScorersDTO> TopScorers { get; set; }
    }
}
