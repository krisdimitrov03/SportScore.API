namespace SportScore.API.DataTransferModels
{
    public class PlayerInTopScorersDTO
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public string Team { get; set; }

        public int Goals { get; set; }

        public int Assists { get; set; }

        public int PenaltyGoals { get; set; }
    }
}
