using System;
using System.Collections.Generic;
using Models.Server;

namespace Server.Models.ServerModels.ChangeTrackingModels
{
    public class GameStateChangesModel 
    {
        public List<TileChangeModel> TileChanges {get;set;}
        public List<WallChangeModel> WallChanges {get;set;}
        public List<ScoreChangeModel> ScoreChanges {get;set;}
    }
}
