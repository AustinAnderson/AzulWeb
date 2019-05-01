using System;
namespace Models.Client
{
    public class ClientRequestModel
    {
        public int PlayerIndex {get;set;}
        public ActionDescriptionModel Action {get;set;}
        public GameStateModel GameState {get;set;}
        public int GameStateHash {get;set;}
    }
}
