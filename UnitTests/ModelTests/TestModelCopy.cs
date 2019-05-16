using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;
using Newtonsoft.Json;
using UnitTests.TestUtils;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class TestModelCopy
    {
        [TestMethod]
        public void TestCopyGameState(){
            GameStateModel model=TestSetupUtils.GetSampleGameStateModel();
            GameStateModel copy=model.DeepCopy();
            Assert.IsFalse(model==copy,"reference equality should be false between original and copy");
            Assert.AreEqual(
                JsonConvert.SerializeObject(model),
                JsonConvert.SerializeObject(copy),
                "should serialize the same"
            );
        }
        
    }
}

