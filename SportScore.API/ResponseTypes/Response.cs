namespace SportScore.API.ResponseTypes
{
    public abstract class Response
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public Response(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
