using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SportScore.API.Constants;
using SportScore.API.DataTransferModels;
using SportScore.API.ResponseTypes;
using SportScore.API.ResponseTypes.User;

namespace SportScore.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("SportScore")]
    //[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class BaseController : Controller
    {
        protected async Task<Response> ValidateAccess<T>(T result)
            where T : class
        {
            if (Request.Headers.ContainsKey(ParamConstants.AuthToken))
            {
                if (Request.Headers[ParamConstants.AuthToken] == Environment.GetEnvironmentVariable("AUTH_TOKEN"))
                    return new SuccessResponse(result);
                else
                    return new AccessDeniedResponse("ACCESS DENIED: Not valid authentication token.");
            }

            return new AccessDeniedResponse();
        }

        protected async Task<Response> ValidateAccess<T>(T result, string authType)
            where T : class
        {
            if (Request.Headers.ContainsKey(ParamConstants.AuthToken))
            {
                if (Request.Headers[ParamConstants.AuthToken] == Environment.GetEnvironmentVariable("AUTH_TOKEN"))
                {
                    if(result == null)
                    {
                        switch (authType)
                        {
                            case "login":
                                return new IncorrectLoginResponse();
                                break;
                            case "register":
                                return new IncorrectRegisterResponse();
                                break;
                            default:
                                return new IncorrectInputResponse();
                                break;
                        }
                    }

                    return new SuccessResponse(result);
                }

                return new AccessDeniedResponse("ACCESS DENIED: Not valid authentication token.");
            }

            return new AccessDeniedResponse();
        }
    }
}
