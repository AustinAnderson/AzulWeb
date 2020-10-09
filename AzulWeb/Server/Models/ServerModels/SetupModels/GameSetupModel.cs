using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models.ServerModels.SetupModels
{
    public class GameSetupModel
    {
        public GameSetupModel(string gameId, List<string> playerIds)
        {
            GameId = gameId;
            PlayerIds = playerIds;
        }

        public string GameId { get; }
        public List<string> PlayerIds { get; }
    }
}
