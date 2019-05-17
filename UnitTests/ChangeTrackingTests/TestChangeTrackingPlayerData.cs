using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;
using Server.Logic.ChangeTracking;
using UnitTests.TestUtils;

namespace UnitTests.ChangeTrackingTests
{
    [TestClass]
    public class TestChangeTrackingPlayerData
    {
        [TestMethod]
        public void FromFactoryToPatternAndFloorLineTracked(){
            TestSetupUtils.FailIfOtherTestsDoesntPass(
                "can't arange test if can't copy",new ModelTests.TestModelCopy().TestCopyGameState
            );
            var beforeState=TestSetupUtils.GetSampleGameStateModel();
            var afterState=beforeState.DeepCopy(); 
            afterState.PlayerData[0].PatternLines[0][0]=afterState.SharedData.Factories[0].TileOne;
            afterState.SharedData.Factories[0].TileOne=null;
            afterState.PlayerData[0].FloorLine[0]=afterState.SharedData.Factories[0].TileThree;
            afterState.SharedData.Factories[0].TileThree=null;
            var tracker=new ChangeTracker();
            var result=tracker.FindChanges(beforeState,afterState);
            Assert.AreEqual(2,result.TileChanges.Count);
            int floorChangeTileId=beforeState.SharedData.Factories[0].TileThree.Id;
            var floorChange=result.TileChanges.FirstOrDefault(x=>
                x.TileId==floorChangeTileId
            );
            if(floorChange==null) Assert.Fail($"expected id {floorChangeTileId} to be one in the change list");
            Assert.AreEqual(
                $"{nameof(GameStateModel.PlayerData)}[0].{nameof(PlayerDataModel.FloorLine)}[0]",
                floorChange.NewJsonPath
            );
            int patternChangeTileId=beforeState.SharedData.Factories[0].TileOne.Id;
            var patternChange=result.TileChanges.FirstOrDefault(x=>
                x.TileId==patternChangeTileId
            );
            if(patternChange==null) Assert.Fail($"expected id {patternChangeTileId} to be one in the change list");
            Assert.AreEqual(
                $"{nameof(GameStateModel.PlayerData)}[0].{nameof(PlayerDataModel.PatternLines)}"+
                $".{nameof(PatternLinesModel.LineOne)}[0]",
                patternChange.NewJsonPath
            );
        }
        [TestMethod]
        public void FromPatternLinesToWall(){
            TestSetupUtils.FailIfOtherTestsDoesntPass(
                "can't arange test if can't copy",new ModelTests.TestModelCopy().TestCopyGameState
            );
            var beforeState=TestSetupUtils.GetSampleGameStateModel();
            var afterState=beforeState.DeepCopy();
            afterState.PlayerData[0].Wall[1][0] = afterState.PlayerData[0].PatternLines[1][0];
            afterState.PlayerData[0].PatternLines[1][0] = null;
            afterState.PlayerData[0].Wall[4][0] = afterState.PlayerData[0].PatternLines[4][2];
            afterState.PlayerData[0].PatternLines[4][2] = null;
            var tracker=new ChangeTracker();
            var result=tracker.FindChanges(beforeState,afterState);
            Assert.AreEqual(2,result.WallChanges.Count,"expected two wall changes" );
            int patternLine1TileId=beforeState.PlayerData[0].PatternLines[1][0].Id;
            var patternLine1WallChange=result.WallChanges.FirstOrDefault(x=>
                x.TileId==patternLine1TileId
            );
            if(patternLine1WallChange==null) Assert.Fail($"expected id {patternLine1TileId} to be one in the change list");
            Assert.AreEqual( 1,patternLine1WallChange.RowIndex,"tile 1 row ndx" );
            Assert.AreEqual( 0,patternLine1WallChange.ColIndex,"tile 1 col ndx" );
            Assert.AreEqual( 0,patternLine1WallChange.PlayerIndex,"tile 1 player ndx" );
            Assert.AreEqual( patternLine1TileId,patternLine1WallChange.TileId,"tile 1 tileId" );

            int patternLine5TileId=beforeState.PlayerData[0].PatternLines[4][2].Id;
            var patternLine5WallChange=result.WallChanges.FirstOrDefault(x=>
                x.TileId==patternLine5TileId
            );
            if(patternLine5WallChange==null) Assert.Fail($"expected id {patternLine5TileId} to be one in the change list");
            Assert.AreEqual( 4,patternLine5WallChange.RowIndex,"tile 2 row ndx" );
            Assert.AreEqual( 0,patternLine5WallChange.ColIndex,"tile 2 col ndx" );
            Assert.AreEqual( 0,patternLine5WallChange.PlayerIndex,"tile 2 player ndx" );
            Assert.AreEqual( patternLine5TileId,patternLine5WallChange.TileId,"tile 2 tileId" );
        }
    }
}

