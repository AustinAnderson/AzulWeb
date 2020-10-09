using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Client;
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
    public class MatchMakingController : ControllerBase
    {
        const string BaseRoute = "api/MatchMaking";
        private GameCreator gameCreator;
        private GameContentHub hub;
        private IPersistenceStore persistence;

        const string NewGameTemplate = "game/{creatorId}";
        [HttpPost(NewGameTemplate)]
        public async Task<ActionResult> GenerateGameId(string creatorId)
        {
            var code=gameCreator.GenerateNewGameCode();
            await persistence.StoreGameSetup(new GameSetupModel(code, new List<string> { creatorId }));
            return await JoinGame(code,creatorId);
        }
        [HttpGet("gameLinkRedirect")]
        public async Task<ActionResult> JoinGame([FromQuery] string gameId,[FromQuery] string playerId)
        {
            var createNewGameBadRequest=new BadRequestModel(
                $"no such game '{gameId} create a new one here",
                (BaseRoute+"/"+NewGameTemplate).Replace("{creatorId}",playerId),
                HttpMethodEnum.POST
            );
            var game = await persistence.GetGameSetup(gameId);
            if (game == null) return BadRequest(createNewGameBadRequest);
            ActionResult response = RedirectPreserveMethod(PagesController.BaseRoute+"/"+gameId);
            if(!game.PlayerIds.Contains(playerId))//player not in already
            {
                if (game.PlayerIds.Count < max)
                {
                    game.PlayerIds.Add(playerId);
                    await persistence.StoreGameSetup(game);
                }
                else
                {
                    createNewGameBadRequest.Message = "The game is full, start a new one here";
                    response = BadRequest(createNewGameBadRequest);
                }
            }
            return response;
        }

        [HttpPost("startGame/{gameId}")]
        public async Task<ActionResult> StartNewGame(string gameId)
        {
            var setup = await persistence.GetGameSetup(gameId);
            if (setup == null)
            {
                return BadRequest(new BadRequestModel(
                    $"no such game '{gameId} create a new one here",
                    (BaseRoute+"/"+NewGameTemplate).Replace("{creatorId}","shrek"),
                    HttpMethodEnum.POST
                ));
            }
            InitialStateModel initialState=gameCreator.GenerateNewGame(
                out Dictionary<string,PlayerIndexAndToken> tokensByConnection,
                gameId,setup.PlayerIds
            );
            await persistence.StoreGameState(initialState.InitialState);
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
