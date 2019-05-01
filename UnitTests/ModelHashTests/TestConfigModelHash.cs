using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;

namespace UnitTests
{
    [TestClass]
    public class TestConfigModelHash
    {
        [TestMethod]
        public void HashCodesDifferentIfDifferentFloorPenalties()
        {
            ConfigModel modelA=new ConfigModel
            {
                FloorPenalties=new int[]{1,2,3,4}
            };
            ConfigModel modelB=new ConfigModel
            {
                FloorPenalties=new int[]{1,2,3,5}
            };
            Assert.AreNotEqual(modelA.GetHashCode(),modelB.GetHashCode());
        }
        [TestMethod]
        public void HashCodesDifferentIfDifferentFloorPenaltiesOrder()
            =>throw new NotImplementedException();
        [TestMethod]
        public void HashCodesDifferentIfDifferentWallPatternByRow()
            =>throw new NotImplementedException();
        [TestMethod]
        public void HashCodesDifferentIfDifferentWallPatternByCol()
            =>throw new NotImplementedException();
        [TestMethod]
        public void HashCodesSameForSameModel()
            =>throw new NotImplementedException();
    }
}
