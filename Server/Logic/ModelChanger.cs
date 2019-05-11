using System;
using System.Collections.Generic;
using Models.Client;
using Models.Server;
using Server.Exceptions;

namespace Server.Logic
{
    public class ModelChanger
    {
        private PlayerTokenMapHandler mapHandler;
        public ModelChanger(PlayerTokenMapHandler mapHandler)
        {
            this.mapHandler=mapHandler;
        }
        //assumes request model has been validated
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
            return stateUpdates;
        }
        private ResponseModel HandleMoveFromFactory(ClientRequestModel request)
        {
            int factoryIndex=request.Action.FactoryIndex;
            int patternLineIndex=request.Action.PatternLineIndex;
            int playerIndex=request.GameState.SharedData.CurrentTurnsPlayersIndex;
            var patternLines=request.GameState.PlayerData[playerIndex].PatternLines;
            var chosenFactory=request.GameState.SharedData.Factories[factoryIndex];
            ResponseModel response=new ResponseModel();
            response.TileChanges=new List<TileChangeModel>();
            for(int i=0;i<chosenFactory.IndexLimit;i++){//foreach tile in the factory
                if(chosenFactory[i]!=null){
                    if(request.Action.TileType==chosenFactory[i].Type){
                        patternLines.AddToLine(patternLineIndex,chosenFactory[i]);
                    }
                    else
                    {
                        request.GameState.SharedData.CenterOfTable.Add()
                    }
                }
            }
        }
        private TileChangeModel AddTileToPlayerBoard(
            List<TileModel> bag,
            PlayerDataModel model,int patternLineIndex,TileModel tile
        )
        {
            var line=model.PatternLines[patternLineIndex];
            bool penalty=true;
            for(int i=0;i<line.Length;i++){
                if(line[i]==null)
                {
                    line[i]=tile;
                    penalty=false;
                    break;
                }
            }
            if(penalty)
            {
                bool toBag=true;
                for(int i=0;i<model.FloorLine.Length;
            }
            return null;
        }

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

