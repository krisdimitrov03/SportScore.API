namespace SportScore.API.DataTransferModels
{
    public class TeamInStandingDTO
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public int GamesPlayed { get; set; }

        public int Wins { get; set; }

        public int Draws { get; set; }

        public int Losts { get; set; }

        public int Points { get; set; }
    }
}
