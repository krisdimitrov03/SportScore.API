using SportScore.API.DataTransferModels;

namespace SportScore.API.SportsDataOperators.Contracts
{
    public interface IUserService
    {
        Task<AuthReturnDTO> LogUserIn(Stream body);
        Task<AuthReturnDTO> RegisterUser(Stream body);
        Task<UserDetailsDTO> GetUserDetails(string id);
    }
}
