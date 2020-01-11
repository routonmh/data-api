using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace
{
    [Route("/")]
    [ApiController]
    public class HomeController
    {

        [HttpGet("/")]
        public ActionResult<string> GetHome()
        {
            string description = "";

            return description;
        }
    }
}