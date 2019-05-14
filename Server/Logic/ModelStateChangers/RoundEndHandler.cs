using System;
using Models.Client;

namespace Server.Logic.ModelStateChangers
{
    public class RoundEndHandler
    {
        public bool ActionEndsRound(ClientRequestModel request)
        {
            //if center of table is empty and all factories are empty, round is done
            bool phaseDone=request.GameState.SharedData.CenterOfTable.Count==0;
            for(int i=0;phaseDone&&i<request.GameState.SharedData.Factories.Length;i++)
            {
                if(!request.GameState.SharedData.Factories[i].IsEmpty) phaseDone=false;
            }
            return phaseDone;
        }
    }
}

