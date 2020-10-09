using Models.Client;
using Models.Server;
using Server.Models.ServerModels.ChangeTrackingModels;
using Server.Models.ServerModels.SetupModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Persistence
{
    public interface IPersistenceStore
    {
        string GenerateId();
        /// <summary>
        /// returns the model with the list of players for the game setup, or null if not found
        /// </summary>
        Task<GameSetupModel> GetGameSetup(string gameId);
        Task StoreGameSetup(GameSetupModel model);

        /// <summary>
        /// returns the current gamestate for an id or null if not found
        /// </summary>
        Task<GameStateModel> GetGameState(string gameId);
        Task StoreGameState(GameStateModel model);

        /// <summary>
        /// returns the list of ui updates for a given game and move for an id or null if not found
        /// </summary>
        Task<ResponseModel> GetMoveUpdates(string gameId, string moveId);
        Task StoreMoveUpdates(string gameId, string moveId, ResponseModel updates);
    }
}
