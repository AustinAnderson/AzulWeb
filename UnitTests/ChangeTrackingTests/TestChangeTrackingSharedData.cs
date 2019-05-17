using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;
using Server.Logic.ChangeTracking;
using UnitTests.TestUtils;

namespace UnitTests.ChangeTrackingTests
{
    [TestClass]
    public class TestChangeTrackingSharedData
    {
        [TestMethod]
        public void DiscardFromPatternLineTracked()
        {
            TestSetupUtils.FailIfOtherTestsDoesntPass(
                "can't arange test if can't copy",new ModelTests.TestModelCopy().TestCopyGameState
            );
            var beforeState=TestSetupUtils.GetSampleGameStateModel();
            var afterState=beforeState.DeepCopy();
            int tId1 = afterState.PlayerData[0].PatternLines[1][0].Id;
            afterState.SharedData.DiscardPile.Add(afterState.PlayerData[0].PatternLines[1][0]);
            afterState.PlayerData[0].PatternLines[1][0] = null;
            int tId2 = afterState.PlayerData[0].PatternLines[4][2].Id;
            afterState.SharedData.DiscardPile.Add(afterState.PlayerData[0].PatternLines[4][2]);
            afterState.PlayerData[0].PatternLines[4][2] = null;

            var tracker=new ChangeTracker();
            var result=tracker.FindChanges(beforeState,afterState);


            Assert.AreEqual(2,result.TileChanges.Count,"expected two tile list changes");
            var patternLine2Change=result.TileChanges.FirstOrDefault(x=>
                x.TileId==tId1
            );
            if(patternLine2Change==null) Assert.Fail($"expected id {tId1} to be one in the change list");
            Assert.AreEqual(
                $"{nameof(GameStateModel.SharedData)}.{nameof(SharedDataModel.DiscardPile)}",
                patternLine2Change.NewJsonPath
            );
            var patternLine5Change=result.TileChanges.FirstOrDefault(x=>
                x.TileId==tId2
            );
            if(patternLine5Change==null) Assert.Fail($"expected id {tId2} to be one in the change list");
            Assert.AreEqual(
                $"{nameof(GameStateModel.SharedData)}.{nameof(SharedDataModel.DiscardPile)}",
                patternLine5Change.NewJsonPath
            );
        }
        [TestMethod]
        public void ScoreChangesTracked()
        {
            TestSetupUtils.FailIfOtherTestsDoesntPass(
                "can't arange test if can't copy",new ModelTests.TestModelCopy().TestCopyGameState
            );
            var beforeState=TestSetupUtils.GetSampleGameStateModel();
            beforeState.PlayerData[0].Score = 5;
            var afterState=beforeState.DeepCopy();
            int p1NewScore= ++afterState.PlayerData[0].Score;
            int p2NewScore= ++afterState.PlayerData[1].Score;
            var tracker=new ChangeTracker();
            var result=tracker.FindChanges(beforeState,afterState);
            Assert.AreEqual(2, result.ScoreChanges.Count, "expected two score changes");
            var p1ScoreChange = result.ScoreChanges.FirstOrDefault(x => x.PlayerIndex == 0);
            Assert.IsNotNull(p1ScoreChange, "expected p1's score chnage to be tracked");
            Assert.AreEqual(p1NewScore, p1ScoreChange.NewScore, "p1's new score");
            var p2ScoreChange = result.ScoreChanges.FirstOrDefault(x => x.PlayerIndex == 1);
            Assert.IsNotNull(p2ScoreChange, "expected p2's score chnage to be tracked");
            Assert.AreEqual(p2NewScore, p2ScoreChange.NewScore, "p2's new score");
            
        }
    }
}

