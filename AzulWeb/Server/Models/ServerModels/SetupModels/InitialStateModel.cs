using System;
using Models.Client;

namespace Server.Models.ServerModels.SetupModels
{
    public class InitialStateModel
    {
        public int InitialHash {get;set;}
        public GameStateModel InitialState {get;set;}
    }
}

