using System;
using Models.Client;
using Models.Server;

namespace Server.Logic
{
    public class ModelChanger
    {
        public ResponseModel ProcessClientChanges(ClientRequestModel request)
        {
            if(request.GameStateHash!=request.GameState.GetHashCode()){
                throw new BadRequestException("client is out of sync with server");
            }
            ResponseModel stateUpdates=new ResponseModel();
            if(request.Action.FromFactory)
            {
                stateUpdates=HandleMoveFromFactory(request);
            }
            else
            {
                stateUpdates=HandleMoveFromCenterTable(request);
            }
            if(request.GameState.SharedData.CenterOfTable.Count==0)
               request.GameState.SharedData.Factories.Count==0
            )
        }
        private ResponseModel HandleMoveFromFactory(ClientRequestModel request)
        {

        }
        private ResponseModel HandleMoveFromCenterTable(ClientRequestModel request)
        {

        }
        private bool ActionFinishesRound(SharedDataModel shared)
        {
            bool roundDone=shared.CenterOfTable.Count==0;
            for(int i=0;roundDone&&i<shared.Factories.Length;i++)
            {
                if(sh)
                for(int j=0;roundDone&&j<shared.Factories[i].Length;j++)
                {
                    if(!shared.Factories[i][j].IsBlank)
                    {
                        
                    }
                }
            }
            return roundDone;
        }
        private void HandleNextRound(ClientRequestModel request)
        {

        }
    }
}

