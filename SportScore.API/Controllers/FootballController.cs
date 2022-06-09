using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;
using SportScore.API.Constants;
using SportScore.API.DataTransferModels;
using SportScore.API.SportsDataOperators.Contracts;

namespace SportScore.API.Controllers
{
    public class FootballController : BaseController
    {
        private readonly IFootballService service;
        public FootballController(IFootballService _service)
        {
            service = _service;
        }

        [HttpGet]
        public async Task<List<FixtureDTO>> Livescore()
        {
            return await service.GetLivescore();
        }

        [HttpGet]
        public async Task<List<FixtureDTO>> Fixtures(string from, string to)
        {
            return await service.GetFixturesByDate(from, to);
        }

        [HttpGet]
        public async Task<List<LeaguesByCountryDTO>> Leagues()
        {
            var leagues = await service.GetLeagues();

            return leagues;
        }
    }
}
