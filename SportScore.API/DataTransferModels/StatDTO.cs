namespace SportScore.API.DataTransferModels
{
    public class StatDTO
    {
        public StatDTO(string type, string home, string away)
        {
            Type = type;
            Home = home;
            Away = away;
        }

        public string Type { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }
    }
}