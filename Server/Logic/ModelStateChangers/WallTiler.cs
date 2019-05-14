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
                for(int j=0;j<playerData.PatternLines[i].Length;j++){
                    if(playerData.PatternLines[i][j]!=null){
                        changes.Add(new TileChangeModel{
                            JsonPath=nameof(GameStateModel.SharedData)+"."+nameof(SharedDataModel.DiscardPile),
                            TileId=playerData.PatternLines[i][j].Id
                        });
                        request.GameState.SharedData.DiscardPile.Add(playerData.PatternLines[i][j]);
                        playerData.PatternLines[i][j]=null;
                    }
                }
            }
        }
        private void AddPlayersChanges(ClientRequestModel request,List<WallChangeModel> toAddTo,int playerIndex){
            var playerData=request.GameState.PlayerData[playerIndex];
            var layout=request.GameState.SharedData.Config.WallLayoutToMatch;
            for(int i=0;i<playerData.PatternLines.IndexLimit;i++){
                if(!playerData.PatternLines[i].IsEmpty)
                {
                    var tile=PluckLastTileInLine(playerData.PatternLines,i);
                    WallChangeModel changed=new WallChangeModel
                    {
                        PlayerIndex=playerIndex,
                        ColIndex=0,
                        RowIndex=i,
                        TileId=tile.Id
                    };
                    for(;changed.ColIndex<layout[i].Length;changed.ColIndex++)
                    {
                        if(layout[i][changed.ColIndex]==tile.Type) break;
                    }
                    //assumes player data wall will match layout
                    playerData.Wall[i][changed.ColIndex]=tile;
                    toAddTo.Add(changed);
                }
            }
        }
        private TileModel PluckLastTileInLine(PatternLinesModel model, int lineIndex)
        {
            var line=model[lineIndex];
            TileModel tile=null;
            for(int i=0;i<line.Length;i++){
                tile=line[i];
                if(i==line.Length-1||line[i+1]==null)
                {
                    line[i]=null;
                    break;
                }
            }
            return tile;
        }
    }
}