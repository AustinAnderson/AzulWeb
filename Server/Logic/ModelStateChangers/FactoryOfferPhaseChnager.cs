using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;
using Models.Server;

namespace Server.Logic.ModelStateChangers
{
    public class FactoryOfferPhaseChnager
    {
        private PlayerTokenMapHandler mapHandler;
        public FactoryOfferPhaseChnager(PlayerTokenMapHandler mapHandler)
        {
            this.mapHandler=mapHandler;
        }
        //assumes request model has been validated
        public List<TileChangeModel> ProcessClientChanges(ClientRequestModel request)
        {
            var tileChanges=new List<TileChangeModel>();
            if(request.Action.FromFactory)
            {
                tileChanges=HandleMoveFromFactory(request);
            }
            else
            {
                tileChanges=HandleMoveFromCenterTable(request);
            }
            return tileChanges;
        }
        private List<TileChangeModel> HandleMoveFromFactory(ClientRequestModel request)
        {
            int factoryIndex=request.Action.FactoryIndex;
            int patternLineIndex=request.Action.PatternLineIndex;
            int playerIndex=request.GameState.SharedData.CurrentTurnsPlayersIndex;
            var patternLines=request.GameState.PlayerData[playerIndex].PatternLines;
            var chosenFactory=request.GameState.SharedData.Factories[factoryIndex];
            var tileChanges=new List<TileChangeModel>();
            for(int i=0;i<chosenFactory.IndexLimit;i++){//foreach tile in the factory
                if(chosenFactory[i]!=null){//if its not an empty factory
                //move the ones requested to the requested patternline/penalty/bag
                //and the others to the center
                    if(request.Action.TileType==chosenFactory[i].Type){
                        tileChanges.Add(CopyTileToPlayerBoardOrBag(
                            request.GameState,playerIndex,patternLineIndex,chosenFactory[i]
                        ));
                    }
                    else
                    {
                        tileChanges.Add(
                            CopyNonChosenTileToCenterOfTable(
                                request.GameState.SharedData.CenterOfTable,
                                chosenFactory[i]
                            )
                        );
                    }
                    chosenFactory[i]=null;
                }
            }
            return tileChanges;
        }
        private List<TileChangeModel> HandleMoveFromCenterTable(ClientRequestModel request)
        {
            var tileChanges=new List<TileChangeModel>();
            tileChanges=new List<TileChangeModel>();
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
                for(int i=0;i<)
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
            return tileChanges;
        }
        private TileChangeModel CopyNonChosenTileToCenterOfTable(HashSet<TileModel> centerOfTable, TileModel tileModel)
        {
            centerOfTable.Add(tileModel);
            return new TileChangeModel{
                NewJsonPath=nameof(GameStateModel.SharedData)+"."+
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
                NewJsonPath=destJPath,
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
                NewJsonPath=nameof(state.PlayerData)+$"[{playerIndex}]"
            };
            var playerData=state.PlayerData[playerIndex];
            var line=playerData.PatternLines[patternLineIndex];
            //add tile to end of the chosen line, or mark that it's full
            bool penalty=true;
            for(int i=0;i<line.Length;i++){
                if(line[i]==null)
                {
                    change.NewJsonPath+="."+nameof(playerData.PatternLines)+
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
    }
}

