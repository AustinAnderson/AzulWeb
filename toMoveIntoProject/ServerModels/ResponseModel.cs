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
        public int NewGameStateHash {get;set;}
        public List<ChangeModel> Changes {get;set;}
    }
}
