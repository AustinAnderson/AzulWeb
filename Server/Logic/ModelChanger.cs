using System;
using Models.Client;
using Models.Server;
using Server.Exceptions;

namespace Server.Logic
{
    public class ModelChanger
    {
        //assumes request model hash been validated
        public ResponseModel ProcessClientChanges(ClientRequestModel request)
        {
            ResponseModel stateUpdates=new ResponseModel();
            if(request.Action.FromFactory)
            {
                stateUpdates=HandleMoveFromFactory(request);
            }
            else
            {
                stateUpdates=HandleMoveFromCenterTable(request);
            }
            throw new NotImplementedException();
        }
        private ResponseModel HandleMoveFromFactory(ClientRequestModel request)
        {
            throw new NotImplementedException();
        }
        private ResponseModel HandleMoveFromCenterTable(ClientRequestModel request)
        {
            throw new NotImplementedException();
        }
        private bool ActionFinishesRound(SharedDataModel shared)
        {
            bool roundDone=shared.CenterOfTable.Count==0;
            for(int i=0;roundDone&&i<shared.Factories.Length;i++)
            {
                if(!shared.Factories[i].IsEmpty) roundDone=false;
            }
            return roundDone;
        }
        private void HandleNextRound(ClientRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}

