using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DataAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController
    {
        [HttpGet]
        public ContentResult GetHome()
        {
            string content = "<h1>.NET Core API</h1>" +
                             "<p>Base Project</p>" +
                             "<a href=\"https://github.com/routonmh/data-api\">GitHub</a>";

            return new ContentResult
            {
                ContentType = "text/html",
                Content = content
            };
        }
    }
}