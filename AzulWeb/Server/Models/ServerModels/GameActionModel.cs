using Models.Client;
using System;
namespace Models.Server
{
    public class GameActionModel
    {

        public GameActionModel(ActionDescriptionModel action, GameStateModel state, int gameStateHash)
        {
            Action = action;
            GameState = state;
            GameStateHash = gameStateHash;
        }

        public ActionDescriptionModel Action { get; set; }
        public GameStateModel GameState { get; set; }
        public int GameStateHash { get; set; }
    }
}
