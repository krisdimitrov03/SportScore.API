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
        public async Task<IActionResult> Livescore()
        {
            return Ok(await ValidateAccess(await service.GetLivescore()));
        }

        [HttpGet]
        public async Task<IActionResult> Fixtures(string from, string to)
        {
            return Ok(await ValidateAccess(await service.GetFixturesByDate(from, to)));
        }

        [HttpGet]
        public async Task<IActionResult> Leagues()
        {
            return Ok(await ValidateAccess(await service.GetLeagues()));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> MatchDetails(string id)
        {
            return Ok(await ValidateAccess(await service.GetMatchDetails(id)));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> LeagueDetails(string id)
        {
            return Ok(await ValidateAccess(await service.GetLeagueDetails(id)));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> TeamDetails(string id)
        {
            return Ok(await ValidateAccess(await service.GetTeamDetails(id)));
        }
    }
}
