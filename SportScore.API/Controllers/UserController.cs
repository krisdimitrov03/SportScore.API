using Microsoft.AspNetCore.Mvc;
using SportScore.API.DataTransferModels;
using SportScore.API.ResponseTypes;
using SportScore.API.SportsDataOperators.Contracts;

namespace SportScore.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService service;

        public UserController(IUserService _service)
        {
            service = _service;
        }

        [HttpPost]
        public async Task<IActionResult> Login()
        {
            //var (user, errors) = await service.LogUserIn(Request.Body);

            //if(user != null)
            //{
            //    var token = service.GenerateToken(user);

            //    return Ok(token);
            //}

            //return NotFound($"User not found: {errors}");
            return Ok(await ValidateAccess(await service.LogUserIn(Request.Body), "login"));
        }

        [HttpPost]
        public async Task<IActionResult> Register()
        {
            return Ok(await ValidateAccess(await service.RegisterUser(Request.Body), "register"));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Info(string id)
        {
            return Ok(await ValidateAccess((await service.GetUserDetails(id), string.Empty), "details"));
        }

        [HttpGet]
        [Route("{token}")]
        public async Task<IActionResult> ForNav(string token)
        {
            return Ok(await ValidateAccess((await service.GetUserByAccessToken(token), string.Empty), "forNav"));
        }
    }
}
