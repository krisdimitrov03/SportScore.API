namespace SportScore.API.ResponseTypes.User
{
    public class IncorrectRegisterResponse : IncorrectAuthResponse
    {
        public IncorrectRegisterResponse() 
            : base("register")
        {
        }
    }
}
