using SportScore.API.DataTransferModels;
using SportScore.Infrastructure.Data;

namespace SportScore.API.SportsDataOperators.Contracts
{
    public interface IUserService
    {
        Task<(UserInNavDTO?, string)> LogUserIn(Stream body);
        Task<(AuthReturnDTO?, string)> RegisterUser(Stream body);
        Task<UserDetailsDTO?> GetUserDetails(string id);
        Task<UserInNavDTO?> GetUserByAccessToken(string id);
        string GenerateToken(UserInNavDTO user);
    }
}
