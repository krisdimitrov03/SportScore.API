namespace SportScore.API.ResponseTypes.User
{
    public abstract class IncorrectAuthResponse : Response
    {
        protected IncorrectAuthResponse(string authType, string errors) 
            : base(0, $"Incorrect {authType} attempt: {errors}", null)
        {
        }
    }
}
