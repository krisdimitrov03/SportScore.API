using Microsoft.AspNetCore.Mvc;
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
        public async Task<Response> Login()
        {
            return await ValidateAccess(await service.LogUserIn(Request.Body), "login");
        }

        [HttpPost]
        public async Task<Response> Register()
        {
            return await ValidateAccess(await service.RegisterUser(Request.Body), "register");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Response> Info(string id)
        {
            return await ValidateAccess(await service.GetUserDetails(id), "details");
        }
    }
}
