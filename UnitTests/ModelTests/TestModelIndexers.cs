using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class TestModelIndexers
    {
        [TestMethod]
        public void FactoryModelCanRetrieveName(){
            FactoryModel model=new FactoryModel();
            Assert.AreEqual(nameof(FactoryModel.TileThree),model.NameOf(2));
        }
        [TestMethod]
        public void FactoryModelCanRetrieveTile(){
            FactoryModel model=new FactoryModel(){
                TileTwo=new TileModel(3,TileType.White)
            };
            var actual=model[1];
            Assert.AreEqual(model.TileTwo,actual,"expected indexing[1] to produce tile two");
        }
        [TestMethod]
        public void FactoryModelCanSetTile(){
            FactoryModel model=new FactoryModel(){
                TileTwo=new TileModel(3,TileType.White)
            };
            TileModel tile=new TileModel(2,TileType.Blue);
            model[1]=tile;
            Assert.AreEqual(tile,model.TileTwo,"expected indexing[1] set to assign tile two");
        }
        [TestMethod]
        public void FactoryModelCanSetTileNull(){
            FactoryModel model=new FactoryModel(){
                TileTwo=new TileModel(3,TileType.White)
            };
            model[1]=null;
            Assert.AreEqual(null,model.TileTwo,"expected indexing[1] set to null to assign null");
        }
        [TestMethod]
        public void FactoryModelThrowsOnOutOfRangeOnGet(){
            FactoryModel model=new FactoryModel();
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>model[model.IndexLimit+1],"for limit+1");
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>model[-1],"for -1");
        }
        [TestMethod]
        public void FactoryModelThrowsOnOutOfRangeOnSet(){
            FactoryModel model=new FactoryModel();
            TileModel t=new TileModel(9,TileType.Black);
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>model[model.IndexLimit+1]=t,"for limit+1");
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>model[-1]=t,"for -1");
        }
        [TestMethod]
        public void FactoryModelThrowsOnOutOfRangeOnNameOf(){
            FactoryModel model=new FactoryModel();
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>model.NameOf(model.IndexLimit+1),"for limit+1");
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>model.NameOf(-1),"for -1");
        }
        [TestMethod]
        public void PatternLinesModelCanRetrieveName(){
            throw new NotImplementedException();
        }
        [TestMethod]
        public void PatternLinesModelCanRetrieveTile(){
            throw new NotImplementedException();
        }
        [TestMethod]
        public void PatternLinesModelCanSetTile(){
            throw new NotImplementedException();
        }
        [TestMethod]
        public void PatternLinesModelThrowsOnOutOfRange(){
            throw new NotImplementedException();
        }
    }
}

