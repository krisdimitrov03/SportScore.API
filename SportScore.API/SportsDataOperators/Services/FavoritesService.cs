using SportScore.API.DataTransferModels;
using SportScore.API.SportsDataOperators.Contracts;
using SportScore.Infrastructure.Data;
using SportScore.Infrastructure.Data.Repositories;

namespace SportScore.API.SportsDataOperators.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly IFootballService footballService;
        private readonly IApplicationDbRepository repo;
        private BodyParser parser;

        public FavoritesService(
            IFootballService _footballService,
            IApplicationDbRepository _repo)
        {
            footballService = _footballService;
            repo = _repo;
            parser = new BodyParser();
        }

        public async Task<FavoriteMatchDTO?> Create(Stream stream)
        {
            var item = await parser.Parse<FavoriteMatchDTO>(stream);

            if (item == null) return null;

            var userFavMatch = Activator.CreateInstance<UserFavouriteMatch>();

            try
            {
                if (await footballService.GetMatchById(item.MatchId) == null)
                {
                    return null;
                }

                userFavMatch.UserId = item.UserId;
                userFavMatch.MatchId = item.MatchId;

                await repo.AddAsync(userFavMatch);
                await repo.SaveChangesAsync();

                return new FavoriteMatchDTO()
                {
                    MatchId = userFavMatch.MatchId,
                    UserId = userFavMatch.UserId
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<MatchDTO>> Get(string userId)
        {
            var result = new List<MatchDTO>();

            var favorites = repo.All<UserFavouriteMatch>()
                .Where(f => f.UserId == userId)
                .ToList();

            try
            {
                foreach (var match in favorites)
                {
                    var matchDTO = await footballService.GetMatchById(match.MatchId);

                    if (matchDTO != null) result.Add(matchDTO);
                }
            }
            catch (Exception) { }

            return result;
        }

        public async Task<FavoriteMatchDTO?> Remove(string id)
        {
            try
            {
                var itemToRemove = repo.All<UserFavouriteMatch>()
                    .FirstOrDefault(f => f.Id == int.Parse(id));

                if (itemToRemove != null)
                {
                    await repo.DeleteAsync<UserFavouriteMatch>(int.Parse(id));
                    await repo.SaveChangesAsync();
                }

                return new FavoriteMatchDTO()
                {
                    MatchId = itemToRemove.MatchId,
                    UserId = itemToRemove.UserId
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
