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
                AddPlayersChanges(request,i);
                AddDiscardRemainingPatternLineTiles(request,i);
            }
        }
        private void AddDiscardRemainingPatternLineTiles(
            ClientRequestModel request,int playerIndex
        )
        {
            var playerData=request.GameState.PlayerData[playerIndex];
            for(int i=0;i<playerData.PatternLines.IndexLimit;i++){
                while(!playerData.PatternLines[i].IsEmpty)
                {
                    var popped=playerData.PatternLines[i].PopOrNull();
                    if(popped!=null) request.GameState.SharedData.DiscardPile.Add(popped);
                }
            }
        }
        private void AddPlayersChanges(ClientRequestModel request,int playerIndex){
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