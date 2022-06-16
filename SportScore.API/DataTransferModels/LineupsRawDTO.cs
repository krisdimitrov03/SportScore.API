namespace SportScore.API.DataTransferModels
{
    public class LineupsRawDTO
    {
        public Dictionary<string, List<Dictionary<string, string>>> Home_Team { get; set; }
        public Dictionary<string, List<Dictionary<string, string>>> Away_Team { get; set; }
    }
}
