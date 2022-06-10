using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;
using SportScore.API.Constants;
using SportScore.API.DataTransferModels;
using SportScore.API.ResponseTypes;
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
        public async Task<Response> Livescore()
        {
            return await ValidateAccess(await service.GetLivescore());
        }

        [HttpGet]
        public async Task<Response> Fixtures(string from, string to)
        {
            return await ValidateAccess(await service.GetFixturesByDate(from, to));
        }

        [HttpGet]
        public async Task<Response> Leagues()
        {
            return await ValidateAccess(await service.GetLeagues());
        }
    }
}
