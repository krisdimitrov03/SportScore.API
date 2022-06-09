using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SportScore.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("SportScore")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class BaseController : Controller
    {
    }
}
