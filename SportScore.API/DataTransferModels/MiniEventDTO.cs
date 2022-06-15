namespace SportScore.API.DataTransferModels
{
    public class MiniEventDTO
    {
        public MiniEventDTO(string time, string type, string side, string playerName, string? result = null)
        {
            Time = time;
            Type = type;
            Side = side;
            PlayerName = playerName;
            Result = result;
        }

        public string Time { get; set; }

        public string Type { get; set; }

        public string Side { get; set; }

        public string PlayerName { get; set; }

        public string? Result { get; set; }
    }
}