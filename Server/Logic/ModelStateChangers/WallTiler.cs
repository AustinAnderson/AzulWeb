using System;
using System.Collections.Generic;
using Models.Client;
using Models.Server;
using Server.Models.ServerModels.SuccessModels;

namespace Server.Logic.ModelStateChangers
{
    public class WallTiler
    {
        public void MovePatternLineTilesToWalls(ClientRequestModel request)
        {
            for(int i=0;i<request.GameState.PlayerData.Count;i++){
                MoveFullPatternLinesToWall(request,i);
            }
        }
        private void DiscardRemainingPatternLineTiles(
            ClientRequestModel request,FixedLengthTileModelQueue patternLine 
        )
        {
            while(!patternLine.IsEmpty)
            {
                var popped=patternLine.PopOrNull();
                if(popped!=null) request.GameState.SharedData.DiscardPile.Add(popped);
            }
        }
        private void MoveFullPatternLinesToWall(ClientRequestModel request,int playerIndex){
            var playerData=request.GameState.PlayerData[playerIndex];
            var layout=request.GameState.SharedData.Config.WallLayoutToMatch;
            for(int i=0;i<playerData.PatternLines.IndexLimit;i++){
                if(playerData.PatternLines[i].IsFull)
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
                    DiscardRemainingPatternLineTiles(request,playerData.PatternLines[i]);
                }
            }
        }
    }
}