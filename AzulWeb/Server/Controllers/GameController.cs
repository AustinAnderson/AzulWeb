using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Client;
using Server.Logic.ModelStateChangers;
using Server.MatchMaking;
using Server.Models.ServerModels.SetupModels;
using Server.SignalRHubs;
            //make new initial game state and broadcast it to the ids

namespace Server.Controllers
{
    [Route(BaseRoute)]
    [ApiController]
    public class GameController : ControllerBase
    {
        const string BaseRoute = "api";
        private GameCreator gameCreator;
        private GameContentHub hub;
        [HttpGet("gameState/{gameId}")]
        public async Task<ActionResult> StartNewGame(string gameId)
        {
            //mongo get state
            return Ok(GameStateModel);
        }
        const string UpdatesTemplate = "updates/{gameId}/{moveId}";
        [HttpGet(UpdatesTemplate)]
        public async Task<ActionResult> GetUpdatesFromMove(string gameId,string moveId)
        {
            //mongo get model from id and moveid
            return Ok(GameStateChangesModel);
        }
        [HttpPost("move/{gameId}")]
        public async Task<ActionResult> DoMove(string gameId, ClientRequestModel model)
        {
            var resp=((ModelChanger)new object()).ProcessRequest(model);
            //mongo store resp with id
            var moveId = "";
            string uri=((BaseRoute + UpdatesTemplate).Replace("gameId", gameId).Replace("moveId", moveId));
            //signalR broadcast uri on Moved channel
            //clients should act on that, not on the response here, which is done for semantic correctness
            return Created(uri, moveId);
        }
    }
}
