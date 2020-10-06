using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route(BaseRoute)]
    [ApiController]
    public class PagesController : ControllerBase
    {
        public const string BaseRoute = "game";
        [HttpGet("{gameId}")]
        public page GetLandingPage(string gameId)
        {
            //if no mongo gameId, generic 404 page
            return "page"
        }

    }
}
