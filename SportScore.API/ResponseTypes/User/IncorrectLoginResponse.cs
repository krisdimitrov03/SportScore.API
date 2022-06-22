namespace SportScore.API.ResponseTypes.User
{
    public class IncorrectLoginResponse : IncorrectAuthResponse
    {
        public IncorrectLoginResponse() 
            : base("log in")
        {
        }
    }
}
