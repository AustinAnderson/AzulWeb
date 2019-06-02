using System;
namespace Models.Client
{
    public class ActionDescriptionModel 
    {
        //to be held in the client and not shown to other clients
        //sent at start of game to each client individually along with the index they are in the array,
        //used as key in encrypted map of token to playerindex in gamestate model
        //this way player knows when SharedState.CurrentTurnsPlayersIndex is this player
        //and can send a request, but while they could guess other player's indexes, they cant
        //fake other player's playerToken
        public Guid PlayerToken {get;set;}
        public bool FromFactory {get;set;}
        public TileType TileType {get;set;}
        public int FactoryIndex {get;set;}
        public int PatternLineIndex {get;set;}
    }
}
