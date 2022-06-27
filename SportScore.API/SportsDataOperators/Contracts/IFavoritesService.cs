using SportScore.API.DataTransferModels;
using SportScore.Infrastructure.Data;

namespace SportScore.API.SportsDataOperators.Contracts
{
    public interface IFavoritesService
    {
        Task<List<MatchDTO>> Get(string userId);
        Task<FavoriteMatchDTO> Create(Stream stream);
        Task<FavoriteMatchDTO> Remove(string id);
    }
}
