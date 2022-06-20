using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SportScore.API.ResponseTypes;
using System.IO;

namespace SportScore.API.Controllers
{
    public class UserController : BaseController
    {
        public async Task<Response> Login()
        {
            string body = "";

            using (var reader = new StreamReader(Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);



            return new SuccessResponse(new Object());
        }
    }
}
