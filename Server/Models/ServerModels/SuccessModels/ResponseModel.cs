using System;
using System.Collections.Generic;
namespace Models.Server
{
    public class ResponseModel
    {
        //calculate the hash of the game state from client,
        //which must match the request model's hash prop
        //then
        //calculate based on executing changes from client
        //and store here, which client stores in it's model and sends back
        //this way gamestate integrety is validated statelessly
        ////each client is responsible for enacting the same changes client side
        public int NewGameStateHash {get;set;}
        public List<TileChangeModel> TileChanges {get;set;}
        public List<WallChangeModel> WallChanges {get;set;}
        public List<ScoreChangeModel> ScoreChanges {get;set;}
    }
}
