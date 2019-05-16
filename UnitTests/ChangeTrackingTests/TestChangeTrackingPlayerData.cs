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
    }
}

