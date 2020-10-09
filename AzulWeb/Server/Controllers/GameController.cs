using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Client;
using Models.Server;
using Server.Logic.ModelStateChangers;
using Server.MatchMaking;
using Server.Models.ServerModels.ErrorModels;
using Server.Models.ServerModels.SetupModels;
using Server.Persistence;
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
        private IPersistenceStore persistence;
        private ModelChanger gameTransitionLogic;
        public const string GetStateTemplate = "gameState/{gameId}";
        [HttpGet(GetStateTemplate)]
        public async Task<ActionResult> GetState(string gameId)
        {
            var state=await persistence.GetGameState(gameId);
            ActionResult response = BadRequest(new BadRequestModel($"no such game '{gameId}'"));
            if (state != null) response = Ok(state);
            return response;
        }
        const string UpdatesTemplate = "updates/{gameId}/{moveId}";
        [HttpGet(UpdatesTemplate)]
        public async Task<ActionResult> GetUpdatesFromMove(string gameId,string moveId)
        {
            //mongo get model from id and moveid
            var updates = await persistence.GetMoveUpdates(gameId, moveId);
            ActionResult response = BadRequest(new BadRequestModel($"no game '{gameId}' with move '{moveId}'"));
            if (updates != null) response = Ok(updates);
            return response;
        }
        [HttpPost("move/{gameId}")]
        public async Task<ActionResult> DoMove(string gameId, [FromBody] ClientRequestModel request)
        {
            var state=await persistence.GetGameState(gameId);
            if (state == null) 
            {
                return BadRequest(new BadRequestModel("no game with id '{gameId}'"));
            }
            if (request.GameStateHash != state.GetHashCode())
            {
                return BadRequest(
                    new BadRequestModel(
                        "client gamestate out of date, update here",
                        BaseRoute+"/"+GetStateTemplate.Replace("gameId",gameId)
                    )
                );
            }
            var updates = gameTransitionLogic.ProcessRequest(request, state);
            var moveId = persistence.GenerateId();
            await persistence.StoreGameState(state);
            await persistence.StoreMoveUpdates(gameId,moveId,updates);
            string uri=((BaseRoute + UpdatesTemplate).Replace("gameId", gameId).Replace("moveId", moveId));
            BroadCaster.Moved(uri, moveId,playerIds);
            //signalR broadcast uri on Moved channel
            //clients should act on that, not on the response here, which is done for semantic correctness
            return Created(uri, moveId);
        }
    }
}
