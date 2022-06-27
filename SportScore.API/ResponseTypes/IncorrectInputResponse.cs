namespace SportScore.API.ResponseTypes
{
    public class IncorrectInputResponse : Response
    {
        public IncorrectInputResponse() 
            : base(0, "Ivalid input!", null)
        {
        }
    }
}
