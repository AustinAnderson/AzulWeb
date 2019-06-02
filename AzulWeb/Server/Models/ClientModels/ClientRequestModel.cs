using System;
namespace Models.Client
{
    public class ClientRequestModel
    {
        public ActionDescriptionModel Action {get;set;}
        public GameStateModel GameState {get;set;}
        public int GameStateHash {get;set;}
    }
}
