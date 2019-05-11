using System;
using System.Collections.Generic;
using System.Linq;
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
            ResponseModel response=new ResponseModel();
            if(request.Action.FromFactory)
            {
                response=HandleMoveFromFactory(request);
            }
            else
            {
                response=HandleMoveFromCenterTable(request);
            }
            response.NewGameStateHash=request.GameState.GetHashCode();
            return response;
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
                if(chosenFactory[i]!=null){//if its not an empty factory
                //move the ones requested to the requested patternline/penalty/bag
                //and the others to the center
                    if(request.Action.TileType==chosenFactory[i].Type){
                        response.TileChanges.Add(CopyTileToPlayerBoardOrBag(
                            request.GameState,playerIndex,patternLineIndex,chosenFactory[i]
                        ));
                    }
                    else
                    {
                        response.TileChanges.Add(
                            CopyNonChosenTileToCenterOfTable(
                                request.GameState.SharedData.CenterOfTable,
                                chosenFactory[i]
                            )
                        );
                    }
                    chosenFactory[i]=null;
                }
            }
            return response;
        }
        private ResponseModel HandleMoveFromCenterTable(ClientRequestModel request)
        {
            ResponseModel response=new ResponseModel();
            response.TileChanges=new List<TileChangeModel>();
            int patternLineIndex=request.Action.PatternLineIndex;
            int playerIndex=request.GameState.SharedData.CurrentTurnsPlayersIndex;
            var patternLines=request.GameState.PlayerData[playerIndex].PatternLines;
            var centerOfTable=request.GameState.SharedData.CenterOfTable;
            //if no one's drawn from the first player spot, we need to add it to this player's penalties
            TileModel firstPlayerToken=centerOfTable.FirstOrDefault(t=>t.Type==TileType.FirstPlayerMarker);
            if(firstPlayerToken!=null)
            {
                response.TileChanges.Add(CopyTileToPlayerPenaltyOrBag(
                    request.GameState,playerIndex,firstPlayerToken
                ));
                centerOfTable.Remove(firstPlayerToken);
            }
            //move chosen tiles to the chosen pattern line
            for(int i=centerOfTable.Count-1;i>0;i--)
            {
                var tile = centerOfTable[i];
                if(tile.Type==request.Action.TileType){
                    response.TileChanges.Add(CopyTileToPlayerBoardOrBag(
                        request.GameState,playerIndex,patternLineIndex,tile
                    ));
                    centerOfTable.Remove(tile);
                }
            }
            return response;
        }
        private TileChangeModel CopyNonChosenTileToCenterOfTable(List<TileModel> centerOfTable, TileModel tileModel)
        {
            centerOfTable.Add(tileModel);
            return new TileChangeModel{
                JsonPath=nameof(GameStateModel.SharedData)+"."+
                         nameof(GameStateModel.SharedData.CenterOfTable),
                TileId=tileModel.Id
            };
        }

        private TileChangeModel CopyTileToPlayerPenaltyOrBag(
            GameStateModel state,int playerIndex, TileModel tile
        )
        {
            var playerData=state.PlayerData[playerIndex];
            string destJPath=nameof(state.PlayerData)+$"[{playerIndex}]";
            bool toBag=true;
            var floorLine=playerData.FloorLine;
            for(int i=0;i<floorLine.Length;i++){
                if(floorLine[i]==null)
                {
                    destJPath+="."+nameof(playerData.FloorLine)+ $"[{i}]";
                    floorLine[i]=tile;
                    break;
                }
            }
            //if penalty floor is full, it goes in the bag
            if(toBag)
            {
                destJPath=nameof(state.SharedData)+"."+nameof(state.SharedData.Bag);
                state.SharedData.Bag.Add(tile);
            }
            return new TileChangeModel{
                JsonPath=destJPath,
                TileId=tile.Id
            };
        }
        private TileChangeModel CopyTileToPlayerBoardOrBag(
            GameStateModel state,int playerIndex,
            int patternLineIndex,TileModel tile
        )
        {
            TileChangeModel change=new TileChangeModel{
                TileId=tile.Id,
                JsonPath=nameof(state.PlayerData)+$"[{playerIndex}]"
            };
            var playerData=state.PlayerData[playerIndex];
            var line=playerData.PatternLines[patternLineIndex];
            //add tile to end of the chosen line, or mark that it's full
            bool penalty=true;
            for(int i=0;i<line.Length;i++){
                if(line[i]==null)
                {
                    change.JsonPath+="."+nameof(playerData.PatternLines)+
                              $".{playerData.PatternLines.NameOf(patternLineIndex)}[{i}]";
                    line[i]=tile;
                    penalty=false;
                    break;
                }
            }
            if(penalty)
            {
                //if its full, add it to the end of the penalty floor, or mark that that's full
                change=CopyTileToPlayerPenaltyOrBag(state,playerIndex,tile);
            }
            return change;
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

