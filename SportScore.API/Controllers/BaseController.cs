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
                //if (Request.Headers[ParamConstants.AuthToken] == Environment.GetEnvironmentVariable("AUTH_TOKEN"))
                if (Request.Headers[ParamConstants.AuthToken] == "9dc97af7-5b7e-466b-9fc1-1ca2873bc4")
                {
                    if (result == null) return new IncorrectInputResponse();
                    else return new SuccessResponse(result);
                }
                else
                    return new AccessDeniedResponse("ACCESS DENIED: Not valid authentication token.");
            }

            return new AccessDeniedResponse();
        }

        protected async Task<Response> ValidateAccess<T>((T, string) result, string authType)
            where T : class
        {
            var (user, message) = result;

            if (Request.Headers.ContainsKey(ParamConstants.AuthToken))
            {
                //if (Request.Headers[ParamConstants.AuthToken] == Environment.GetEnvironmentVariable("AUTH_TOKEN"))
                if (Request.Headers[ParamConstants.AuthToken] == "9dc97af7-5b7e-466b-9fc1-1ca2873bc4")
                {
                    if (user == null)
                    {
                        Response response;

                        switch (authType)
                        {
                            case "login":
                                response = new IncorrectLoginResponse(message);
                                break;
                            case "register":
                                response = new IncorrectRegisterResponse(message);
                                break;
                            default:
                                response = new IncorrectInputResponse();
                                break;
                        }

                        return response;
                    }

                    return new SuccessResponse(user);
                }

                return new AccessDeniedResponse("ACCESS DENIED: Not valid authentication token.");
            }

            return new AccessDeniedResponse();
        }
    }
}
