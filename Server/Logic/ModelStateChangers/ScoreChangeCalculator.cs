using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;
using Models.Server;

namespace Server.Logic.ModelStateChangers
{
    public class ScoreChangeCalculator
    {
        public void UpdateScores(ClientRequestModel request,List<WallChangeModel> newAdditions){
            for(int i=0;i<request.GameState.PlayerData.Count;i++)
            {
                UpdateScoreForPlayer(
                    request.GameState.PlayerData[i],
                    newAdditions.Where(change=>change.PlayerIndex==i)
                );
                DecrementScoreFromFloorPenalties(
                    request.GameState.PlayerData[i],
                    request.GameState.SharedData.Config
                );
            }
        }
        public void AddFinalBonusesToScore(ClientRequestModel request){
            
        }
        private void UpdateScoreForPlayer(PlayerDataModel playerData,IEnumerable<WallChangeModel> changesForPlayer)
        {
            for(int row=0;row<playerData.Wall.Length;row++){
                UpdateChangesForRow(playerData,
                    changesForPlayer.FirstOrDefault(change=>change.RowIndex==row),
                    changesForPlayer.Where(change=>change.RowIndex>row)
                );
            }
        }
        private void UpdateChangesForRow(PlayerDataModel playerData, 
            WallChangeModel rowSeedTile,
            IEnumerable<WallChangeModel> IgnoredTilesForRow
        )
        {
            if(rowSeedTile!=null)//no change this row
            {
                //handle points for row
                for(int r=0;r<playerData.Wall.Length;r++)
                {
                    if(playerData.Wall[r][rowSeedTile.ColIndex]!=null) 
                        playerData.Score++;
                }
                //handle points for columns
                for(int c=0;c<playerData.Wall[0].Length;c++)
                {
                    if(playerData.Wall[rowSeedTile.RowIndex][c]!=null&&
                        !IgnoredTilesForRow.Any(change=>
                            change.RowIndex==rowSeedTile.RowIndex&&
                            change.ColIndex==c
                        )
                    ){
                        playerData.Score++;
                    }
                }
            }
        }
        private void DecrementScoreFromFloorPenalties(PlayerDataModel playerData,ConfigModel config){
            for(int i=0;i<playerData.FloorLine.Count;i++){
                if(playerData.FloorLine[i]!=null){
                                             //should be a negative int
                    playerData.Score+=config.FloorPenalties[i];
                }
            }
        }
    }
}

