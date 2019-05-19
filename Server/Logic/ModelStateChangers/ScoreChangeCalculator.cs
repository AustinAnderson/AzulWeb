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
            //TODO: how to best display where the bonuses came from and what they were
            for(int i=0;i<request.GameState.PlayerData.Count;i++){
                var playerData=request.GameState.PlayerData[i];
                var playerWall=playerData.Wall;
                for(int r=0;r<playerWall.Length;r++){
                    if(WallRowFilled(playerWall,r)){
                        playerData.Score+=request.GameState.SharedData.Config.RowBonus;
                    } 
                }
                for(int c=0;c<playerWall[0].Length;c++)
                {
                    if(WallColFilled(playerWall,c)){
                        playerData.Score+=request.GameState.SharedData.Config.ColBonus;
                    } 
                }
                foreach(int type in Enum.GetValues(typeof(TileType))){
                    if(AllOfKindFilled(playerWall,type)){
                        playerData.Score+=request.GameState.SharedData.Config.AllOfKindBonus;
                    }
                }
            }
        }

        //since the wall cant be jagged and a type cant be repeated in a row,
        //the number of of a given type is equal to the number of rows
        private bool AllOfKindFilled(TileModel[][] playerWall,int type){
            int typeCount=0;
            for(int r=0;r<playerWall.Length;r++){
                for(int c=0;c<playerWall[r].Length;c++){
                    if(playerWall[r][c]!=null&&(int)playerWall[r][c].Type==type){
                        typeCount++;
                    }
                }
            }
            return typeCount==playerWall.Length;
        }

        private bool WallColFilled(TileModel[][] wall, int colIndex){
            bool filled=true;
            for(int i=0;i<wall.Length;i++)
            {
                if(wall[i][colIndex]==null) filled=false;
            }
            return filled;
        }
        private bool WallRowFilled(TileModel[][] wall, int rowIndex){
            bool filled=true;
            for(int i=0;i<wall[rowIndex].Length;i++)
            {
                if(wall[rowIndex][i]==null) filled=false;
            }
            return filled;
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

