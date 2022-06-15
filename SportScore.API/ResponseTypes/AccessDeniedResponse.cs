namespace SportScore.API.ResponseTypes
{
    public class AccessDeniedResponse : Response
    {
        public AccessDeniedResponse(string? message = null)
            :base(0, "ACCESS DENIED: You need to send your authentication token to access this feature.", null)
        {
            if(message != null)
            {
                base.Message = message;
            }
        }
    }
}
