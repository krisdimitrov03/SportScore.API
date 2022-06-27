namespace SportScore.API.ResponseTypes
{
    public class IncorrectInputResponse : Response
    {
        public IncorrectInputResponse(string? message = null) 
            : base(0, $"Ivalid input!", null)
        {
            if (message != null)
            {
                this.Message += $" {message}";
            }
        }
    }
}
