namespace SportScore.API.ResponseTypes
{
    public class SuccessResponse : Response
    {
        public SuccessResponse(object data)
            :base(1, "Success", data)
        {
        }
    }
}
