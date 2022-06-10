namespace SportScore.API.ResponseTypes
{
    public class AccessDeniedResponse : Response
    {
        public AccessDeniedResponse()
            :base(0, "ACCESS DENIED: You need to send your authentication token to access this feature.", null)
        {
        }
    }
}
