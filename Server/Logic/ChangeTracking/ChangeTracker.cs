using System;
using System.Collections.Generic;
using Models.Client;
using Models.Server;
using Server.Models.ServerModels.ChangeTrackingModels;
namespace Server.Logic.ChangeTracking
{
    public class ChangeTracker
    {
        //loop over each level of before and after, if its in after but not before it was moved
        //if it's in before but not after it was moved somewhere else and the method for a different level
        //will get it
        public GameStateChangesModel FindChanges(GameStateModel before,GameStateModel after)
        {
            GameStateChangesModel changes=new GameStateChangesModel{
                TileChanges=new List<TileChangeModel>(),
                ScoreChanges=new List<ScoreChangeModel>(),
                WallChanges=new List<WallChangeModel>()
            };
            FillSharedDataChanges(changes,before,after);
            for(int i=0;i<after.PlayerData.Count;i++){
                if(i>=before.PlayerData.Count){
                    throw new ArgumentException("before model's player count is less than after model's");
                }
                FillPlayerDataChanges(
                    changes,$"{nameof(GameStateModel.PlayerData)}[{i}]",
                    i,
                    before.PlayerData[i],after.PlayerData[i]
                );
            }
            return changes;
        }
        private void FillPlayerDataChanges(
            GameStateChangesModel changes, string basePath,int playerIndex, 
            PlayerDataModel before, PlayerDataModel after
        )
        {
            if(before.Score!=after.Score){
                changes.ScoreChanges.Add(new ScoreChangeModel{
                    NewScore=after.Score,
                    PlayerIndex=playerIndex
                });
            }
            FillWallChanges(changes,playerIndex,before.Wall,after.Wall);
            FillFixedArrayChanges(
                changes,basePath+$".{nameof(PlayerDataModel.FloorLine)}",
                before.FloorLine,after.FloorLine
            );
            FillPatternLineChanges(
                changes,basePath+$".{nameof(PlayerDataModel.PatternLines)}",
                before.PatternLines,after.PatternLines
            );
        }

        private void FillPatternLineChanges(GameStateChangesModel changes, string basePath, PatternLinesModel before, PatternLinesModel after)
        {
            for(int i=0;i<after.IndexLimit;i++){
                FillFixedArrayChanges(
                    changes,basePath+$".{after.NameOf(i)}",
                    before[i],after[i]
                );
            }
        }
        private void FillWallChanges(
            GameStateChangesModel changes,int playerIndex,TileModel[][] before, TileModel[][] after
        )
        {
            if(before.Length!=after.Length) {
                throw new ArgumentException(
                    $"Wall dimensions somehow changed from {before.Length} rows to {after.Length} rows"
                );
            }
            for(int row=0;row<after.Length;row++){
                if(after[row].Length!=before[row].Length){
                    throw new ArgumentException(
                        $"Wall's col dimensions somehow changed on row {row} from {before[row].Length} to {after[row].Length}"
                    );
                }
                for(int col=0;col<after[row].Length;col++){
                    if(after[row][col]!=null&&after[row][col].Id!=before[row][col]?.Id){
                        changes.WallChanges.Add(new WallChangeModel{
                            RowIndex=row,
                            ColIndex=col,
                            PlayerIndex=playerIndex,
                            TileId=after[row][col].Id,
                        });
                    }
                }
            }
        }
        private void FillSharedDataChanges(GameStateChangesModel changes, GameStateModel before, GameStateModel after)
        {
            for(int i=0;i<after.SharedData.Factories.Length;i++){
                if(i>=before.SharedData.Factories.Length) {
                    throw new ArgumentException("before model's factory count is less than after model's");
                }
                FillFactoryChanges(
                    changes,$"{nameof(GameStateModel.SharedData)}.{nameof(SharedDataModel.Factories)}[{i}]",
                    before.SharedData.Factories[i],after.SharedData.Factories[i]
                );
            }
            FillSetChanges(
                changes,$"{nameof(GameStateModel.SharedData)}.{nameof(SharedDataModel.Bag)}",
                before.SharedData.Bag,after.SharedData.Bag
            );
            FillSetChanges(
                changes,$"{nameof(GameStateModel.SharedData)}.{nameof(SharedDataModel.CenterOfTable)}",
                before.SharedData.CenterOfTable,after.SharedData.CenterOfTable
            );
            FillSetChanges(
                changes,$"{nameof(GameStateModel.SharedData)}.{nameof(SharedDataModel.DiscardPile)}",
                before.SharedData.DiscardPile,after.SharedData.DiscardPile
            );
        }
        private void FillFactoryChanges
        (
            GameStateChangesModel changes, string basePath, FactoryModel before, FactoryModel after
        )
        {
            for(int i=0;i<after.IndexLimit;i++){
                if(after[i]!=null&&after[i].Id!=before[i]?.Id){
                    changes.TileChanges.Add(new TileChangeModel{
                        NewJsonPath=basePath+$".{after.NameOf(i)}",
                        TileId=after[i].Id
                    });
                }
            }
        }
        private void FillFixedArrayChanges
        (
            GameStateChangesModel changes, string basePath, TileModel[] before, TileModel[] after
        )
        {
            if(before.Length!=after.Length)
            {
                throw new ArgumentException($"old {basePath} length of {before.Length} doesn't match after of {after.Length}");
            }
            for(int i=0;i<after.Length;i++){
                if(after[i]!=null&&after[i].Id!=before[i]?.Id){
                    changes.TileChanges.Add(new TileChangeModel{
                        TileId=after[i].Id,
                        NewJsonPath=basePath+$"[{i}]"
                    });
                }
            }
        }
        private void FillSetChanges(
            GameStateChangesModel changes, string basePath, ISet<TileModel> before, ISet<TileModel> after
        )
        {
            //foreach tile in after, if before doesn't have it add it to change list
            foreach(var afterTile in after){
                bool matchingTileFound=false;
                foreach(var beforeTile in before){
                    if(afterTile.Id==beforeTile.Id){
                        matchingTileFound=true;
                        break;
                    }
                }
                if(!matchingTileFound){
                    changes.TileChanges.Add(new TileChangeModel{
                        NewJsonPath=basePath,
                        TileId=afterTile.Id
                    });
                }
            }
        }
    }
}

