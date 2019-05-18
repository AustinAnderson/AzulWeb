using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;
using Models.Server;

namespace Server.Logic.ModelStateChangers
{
    public class FactoryOfferPhaseChanger
    {
        private PlayerTokenMapHandler mapHandler;
        public FactoryOfferPhaseChanger(PlayerTokenMapHandler mapHandler)
        {
            this.mapHandler=mapHandler;
        }
        //assumes request model has been validated
        public void ProcessClientChanges(ClientRequestModel request)
        {
            if(request.Action.FromFactory)
            {
                HandleMoveFromFactory(request);
            }
            else
            {
                HandleMoveFromCenterTable(request);
            }
        }
        private void HandleMoveFromFactory(ClientRequestModel request)
        {
            int factoryIndex=request.Action.FactoryIndex;
            int patternLineIndex=request.Action.PatternLineIndex;
            int playerIndex=request.GameState.SharedData.CurrentTurnsPlayersIndex;
            var patternLines=request.GameState.PlayerData[playerIndex].PatternLines;
            var chosenFactory=request.GameState.SharedData.Factories[factoryIndex];
            for(int i=0;i<chosenFactory.IndexLimit;i++){//foreach tile in the factory
                if(chosenFactory[i]!=null){//if its not an empty factory
                //move the ones requested to the requested patternline/penalty/bag
                //and the others to the center
                    if(request.Action.TileType==chosenFactory[i].Type){
                        CopyTileToPlayerBoardOrBag(
                            request.GameState,playerIndex,patternLineIndex,chosenFactory[i]
                        );
                    }
                    else
                    {
                        request.GameState.SharedData.CenterOfTable.Add(chosenFactory[i]);
                    }
                    chosenFactory[i]=null;
                }
            }
        }
        private void HandleMoveFromCenterTable(ClientRequestModel request)
        {
            int patternLineIndex=request.Action.PatternLineIndex;
            int playerIndex=request.GameState.SharedData.CurrentTurnsPlayersIndex;
            var patternLines=request.GameState.PlayerData[playerIndex].PatternLines;
            var floorLine=request.GameState.PlayerData[playerIndex].FloorLine;
            var centerOfTable=request.GameState.SharedData.CenterOfTable;
            //if no one's drawn from the first player spot, we need to add it to this player's penalties
            TileModel firstPlayerToken=centerOfTable.FirstOrDefault(t=>t.Type==TileType.FirstPlayerMarker);
            if(firstPlayerToken!=null)
            {
                centerOfTable.Remove(firstPlayerToken);
                if(!floorLine.TryAdd(firstPlayerToken)){
                    request.GameState.SharedData.Bag.Add(firstPlayerToken);
                }
            }
            //move chosen tiles to the chosen pattern line
            HashSet<TileModel> newCenterOfTable=new HashSet<TileModel>();
            foreach(var tile in centerOfTable)
            {
                if(tile.Type!=request.Action.TileType)
                {
                    newCenterOfTable.Add(tile);
                }
            }
            centerOfTable=newCenterOfTable;
        }

        private void CopyTileToPlayerBoardOrBag(
            GameStateModel state,int playerIndex,
            int patternLineIndex,TileModel tile
        )
        {
            var playerData=state.PlayerData[playerIndex];
            var line=playerData.PatternLines[patternLineIndex];
            //add tile to end of the chosen line, or mark that it's full
            if(!line.TryAdd(tile))
            {
                //if its full, add it to the end of the penalty floor, or mark that that's full
                if(!playerData.FloorLine.TryAdd(tile))
                {
                    //if penalties full, add it to discard
                    state.SharedData.DiscardPile.Add(tile);
                }
            }
        }
    }
}

