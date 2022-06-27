namespace SportScore.API.ResponseTypes.User
{
    public class IncorrectRegisterResponse : IncorrectAuthResponse
    {
        public IncorrectRegisterResponse(string errors)
            : base("register", errors)
        {
        }
    }
}
