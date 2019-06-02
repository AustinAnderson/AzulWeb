using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Client;
using Server.MatchMaking;
using Server.Models.ServerModels.SetupModels;
using Server.SignalRHubs;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchMakingController : ControllerBase
    {
        private GameCreator gameCreator;
        private GameContentHub hub;
        // GET api/values
        [HttpGet("requestNewGame")]
        public ActionResult<string> GenerateGameId()
        {
            return Ok(gameCreator.GenerateNewGameCode());
        }

        [HttpPost("startNewGame/{gameId}")]
        public async Task<ActionResult> StartNewGame(string gameId,[FromBody] List<string> connectionIds)
        {
            //make new initial game state and broadcast it to the ids
            InitialStateModel initialState=gameCreator.GenerateNewGame(
                out Dictionary<string,PlayerIndexAndToken> tokensByConnection,
                gameId,connectionIds
            );
            foreach(var kvp in tokensByConnection)
            {
                await hub.SendToken(kvp.Key,kvp.Value);
            }
            await hub.SendGameStart(initialState);
            return Ok();
        }
    }
}
