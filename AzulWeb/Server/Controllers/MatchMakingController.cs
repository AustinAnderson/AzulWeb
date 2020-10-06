using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Client;
using Server.MatchMaking;
using Server.Models.ServerModels.SetupModels;
using Server.SignalRHubs;
            //make new initial game state and broadcast it to the ids

namespace Server.Controllers
{
    [Route(BaseRoute)]
    [ApiController]
    public class MatchMakingController : ControllerBase
    {
        const string BaseRoute = "api/MatchMaking";
        private GameCreator gameCreator;
        private GameContentHub hub;
        // GET api/values
        [HttpPost("game/{creatorId}")]
        public ActionResult<string> GenerateGameId(string creatorId)
        {
            var code=gameCreator.GenerateNewGameCode();
            //mongo store simple model of desired game, with id and list of players to be filled
            return JoinGame(code,creatorId);
        }
        [HttpGet("gameLinkRedirect")]
        public ActionResult JoinGame([FromQuery] string gameId,[FromQuery] string playerId)
        {
            if (!gameId)
            {
                return BadRequest($"no such game '{gameId}'");
            }
            //mongo save adding {playerId} to game
            return RedirectPermanentPreserveMethod(PagesController.BaseRoute+"/"+gameId);
        }

        [HttpPost("startGame/{gameId}")]
        public async Task<ActionResult> StartNewGame(string gameId)
        {
            InitialStateModel initialState=gameCreator.GenerateNewGame(
                out Dictionary<string,PlayerIndexAndToken> tokensByConnection,
                gameId,connectionIds
            );
            //mongo store state
            foreach(var kvp in tokensByConnection)
            {
                await hub.SendToken(kvp.Key,kvp.Value);
                //when clients get this update, they hit the get endpoint
                //on the gameController
            }
            return Ok();
        }
    }
}
