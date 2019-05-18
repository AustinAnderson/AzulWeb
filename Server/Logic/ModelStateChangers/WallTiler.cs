using System;
using System.Collections.Generic;
using Models.Client;
using Models.Server;
using Server.Models.ServerModels.SuccessModels;

namespace Server.Logic.ModelStateChangers
{
    public class WallTiler
    {
        public WallMovePhaseModel MovePatternLineTilesToWalls(ClientRequestModel request)
        {
            WallMovePhaseModel wallPhaseChanges=new WallMovePhaseModel();
            wallPhaseChanges.WallChanges=new List<WallChangeModel>();
            wallPhaseChanges.Discards=new List<TileChangeModel>();
            for(int i=0;i<request.GameState.PlayerData.Count;i++){
                AddPlayersChanges(request,wallPhaseChanges.WallChanges,i);
                AddDiscardRemainingPatternLineTiles(request,wallPhaseChanges.Discards,i);
            }
            return wallPhaseChanges;
        }
        private void AddDiscardRemainingPatternLineTiles(
            ClientRequestModel request,List<TileChangeModel> changes,int playerIndex
        )
        {
            var playerData=request.GameState.PlayerData[playerIndex];
            for(int i=0;i<playerData.PatternLines.IndexLimit;i++){
                while(!playerData.PatternLines[i].IsEmpty)
                {
                    var popped=playerData.PatternLines[i].PopOrNull();
                    changes.Add(new TileChangeModel{
                        NewJsonPath=nameof(GameStateModel.SharedData)+"."+nameof(SharedDataModel.DiscardPile),
                        TileId=popped.Id
                    });
                    request.GameState.SharedData.DiscardPile.Add(popped);
                }
            }
        }
        private void AddPlayersChanges(ClientRequestModel request,List<WallChangeModel> toAddTo,int playerIndex){
            var playerData=request.GameState.PlayerData[playerIndex];
            var layout=request.GameState.SharedData.Config.WallLayoutToMatch;
            for(int i=0;i<playerData.PatternLines.IndexLimit;i++){
                if(!playerData.PatternLines[i].IsEmpty)
                {
                    var tile=playerData.PatternLines[i].PopOrNull();
                    for(int colNdx=0;colNdx<layout[i].Length;colNdx++)
                    {
                        if(layout[i][colNdx]==tile.Type)
                        {
                            //assumes player data wall will match layout
                            playerData.Wall[i][colNdx]=tile;
                            break;
                        } 
                    }
                }
            }
        }
    }
}