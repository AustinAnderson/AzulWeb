using System;
using System.Collections.Generic;
using Models.Server;

namespace Server.Models.ServerModels.SuccessModels
{
    public class WallMovePhaseModel
    {
        public List<WallChangeModel> WallChanges {get;set;}
        public List<TileChangeModel> Discards {get;set;}
    }
}

