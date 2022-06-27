using SportScore.API.DataTransferModels;

namespace SportScore.API.SportsDataOperators.Contracts
{
    public interface IUserService
    {
        Task<(AuthReturnDTO?, string)> LogUserIn(Stream body);
        Task<(AuthReturnDTO?, string)> RegisterUser(Stream body);
        Task<UserDetailsDTO?> GetUserDetails(string id);
        Task<UserInNavDTO?> GetUserById(string id);
    }
}
