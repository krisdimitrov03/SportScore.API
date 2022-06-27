namespace SportScore.API.DataTransferModels
{
    public class RegisterDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? ProfileImage { get; set; } = null;

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
