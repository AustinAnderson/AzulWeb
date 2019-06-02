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
        {
            ConfigModel modelA=new ConfigModel
            {
                FloorPenalties=new int[]{1,2,3,4}
            };
            ConfigModel modelB=new ConfigModel
            {
                FloorPenalties=new int[]{1,4,2,3}
            };
            Assert.AreNotEqual(modelA.GetHashCode(),modelB.GetHashCode());
        }
        [TestMethod]
        public void HashCodesDifferentIfDifferentWallPatternByCol()
        {
            ConfigModel modelA=new ConfigModel
            {
                WallLayoutToMatch=new TileType[][]{
                    new TileType[]{TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow,TileType.White},
                    new TileType[]{TileType.Black,TileType.Red,TileType.Yellow,TileType.White,TileType.Blue},
                    new TileType[]{TileType.Red,TileType.Yellow,TileType.White,TileType.Blue,TileType.Black},
                    new TileType[]{TileType.Yellow,TileType.White,TileType.Blue,TileType.Black,TileType.Red},
                    new TileType[]{TileType.White,TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow}
                }
            };
            ConfigModel modelB=new ConfigModel
            {
                WallLayoutToMatch=new TileType[][]{
                    new TileType[]{TileType.Red,TileType.Blue,TileType.Black,TileType.Yellow,TileType.White},
                    new TileType[]{TileType.Black,TileType.Red,TileType.Yellow,TileType.White,TileType.Blue},
                    new TileType[]{TileType.Red,TileType.Yellow,TileType.White,TileType.Blue,TileType.Black},
                    new TileType[]{TileType.Yellow,TileType.White,TileType.Blue,TileType.Black,TileType.Red},
                    new TileType[]{TileType.White,TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow}
                }
            };
            Assert.AreNotEqual(modelA.GetHashCode(),modelB.GetHashCode());
        }
        [TestMethod]
        public void HashCodesDifferentIfDifferentWallPatternByRow()
        {
            ConfigModel modelA=new ConfigModel
            {
                WallLayoutToMatch=new TileType[][]{
                    new TileType[]{TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow,TileType.White},
                    new TileType[]{TileType.Black,TileType.Red,TileType.Yellow,TileType.White,TileType.Blue},
                    new TileType[]{TileType.Red,TileType.Yellow,TileType.White,TileType.Blue,TileType.Black},
                    new TileType[]{TileType.Yellow,TileType.White,TileType.Blue,TileType.Black,TileType.Red},
                    new TileType[]{TileType.White,TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow}
                }
            };
            ConfigModel modelB=new ConfigModel
            {
                WallLayoutToMatch=new TileType[][]{
                    new TileType[]{TileType.Black,TileType.Red,TileType.Yellow,TileType.White,TileType.Blue},
                    new TileType[]{TileType.Yellow,TileType.White,TileType.Blue,TileType.Black,TileType.Red},
                    new TileType[]{TileType.Red,TileType.Yellow,TileType.White,TileType.Blue,TileType.Black},
                    new TileType[]{TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow,TileType.White},
                    new TileType[]{TileType.White,TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow}
                }
            };
            Assert.AreNotEqual(modelA.GetHashCode(),modelB.GetHashCode());
        }
        [TestMethod]
        public void HashCodesSameForSameModel()
        {
            ConfigModel modelA=new ConfigModel
            {
                FloorPenalties=new int[]{1,2,3,4},
                WallLayoutToMatch=new TileType[][]{
                    new TileType[]{TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow,TileType.White},
                    new TileType[]{TileType.Black,TileType.Red,TileType.Yellow,TileType.White,TileType.Blue},
                    new TileType[]{TileType.Red,TileType.Yellow,TileType.White,TileType.Blue,TileType.Black},
                    new TileType[]{TileType.Yellow,TileType.White,TileType.Blue,TileType.Black,TileType.Red},
                    new TileType[]{TileType.White,TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow}
                }
            };
            ConfigModel modelB=new ConfigModel
            {
                FloorPenalties=new int[]{1,2,3,4},
                WallLayoutToMatch=new TileType[][]{
                    new TileType[]{TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow,TileType.White},
                    new TileType[]{TileType.Black,TileType.Red,TileType.Yellow,TileType.White,TileType.Blue},
                    new TileType[]{TileType.Red,TileType.Yellow,TileType.White,TileType.Blue,TileType.Black},
                    new TileType[]{TileType.Yellow,TileType.White,TileType.Blue,TileType.Black,TileType.Red},
                    new TileType[]{TileType.White,TileType.Blue,TileType.Black,TileType.Red,TileType.Yellow}
                }
            };
            Assert.AreEqual(modelA.GetHashCode(),modelB.GetHashCode());
        }
    }
}
