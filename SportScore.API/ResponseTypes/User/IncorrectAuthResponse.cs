namespace SportScore.API.ResponseTypes.User
{
    public abstract class IncorrectAuthResponse : Response
    {
        protected IncorrectAuthResponse(string authType) 
            : base(0, $"Incorrect {authType} attempt.", null)
        {
        }
    }
}
