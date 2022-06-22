namespace SportScore.API.DataTransferModels
{
    public class LoginDTO
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
