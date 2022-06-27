using Microsoft.AspNetCore.Mvc;
using SportScore.API.ResponseTypes;
using SportScore.API.SportsDataOperators.Contracts;

namespace SportScore.API.Controllers
{
    public class FavoritesController : BaseController
    {
        private readonly IFavoritesService service;

        public FavoritesController(IFavoritesService _service)
        {
            service = _service;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<Response> GetByUser(string userId)
        {
            return await ValidateAccess(await service.Get(userId));
        }

        [HttpPost]
        public async Task<Response> Add()
        {
            return await ValidateAccess(await service.Create(Request.Body));
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<Response> Delete(string id)
        {
            return await ValidateAccess(await service.Remove(id));
        }
    }
}
