namespace SportScore.API.ResponseTypes.User
{
    public class IncorrectLoginResponse : IncorrectAuthResponse
    {
        public IncorrectLoginResponse(string errors) 
            : base("log in", errors)
        {
        }
    }
}
