using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;
using Newtonsoft.Json;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class TestCustomModelSerialization
    {
        [TestMethod]
        public void FixedLengthTileModelQueueDeserializes(){
            string json="[{'Id':2,'Type':'Blue'},{'Id':3,'Type':'Blue'},null]";
            var model=JsonConvert.DeserializeObject<FixedLengthTileModelQueue>(json);
        }
        [TestMethod]
        public void PatternLineDeserializes()
        {
            string json="{"+
                "'LineOne': [null],"+
                "'LineTwo': [{'Id':2,'Type':'Blue'},{'Id':3,'Type':'Blue'}],"+
                "'LineThree': [{'Id':4,'Type':'Blue'},{'Id':5,'Type':'Blue'},null],"+
                "'LineFour': [{'Id':6,'Type':'Blue'},{'Id':7,'Type':'Blue'},null,null],"+
                "'LineFive': [{'Id':8,'Type':'Black'},{'Id':9,'Type':'Black'},{'Id':10,'Type':'Black'},null,null]"+
            "}";
            var model=JsonConvert.DeserializeObject<PatternLinesModel>(json);
            AssertLinesEqual(model.LineOne,new TileModel[]{null},"LineOne");
            AssertLinesEqual(model.LineTwo,new TileModel[]{
                new TileModel(2,TileType.Blue), new TileModel(3,TileType.Blue)
            },"LineTwo");
            AssertLinesEqual(model.LineThree,new TileModel[]{
                new TileModel(4,TileType.Blue), new TileModel(5,TileType.Blue),null
            },"LineThree");
            AssertLinesEqual(model.LineFour,new TileModel[]{
                new TileModel(6,TileType.Blue), new TileModel(7,TileType.Blue),null,null
            },"LineFour");
            AssertLinesEqual(model.LineFive,new TileModel[]{
                new TileModel(8,TileType.Black), new TileModel(9,TileType.Black),new TileModel(10,TileType.Black),
                null,null
            },"LineFive");
        }
        private void AssertLinesEqual(FixedLengthTileModelQueue deserialized,TileModel[] expected,string lineName){
            Assert.AreEqual(expected.Length,deserialized.Count,lineName+".Count");
            for(int i=0;i<expected.Length;i++){

                Assert.AreEqual(getStringRep(expected[i]),getStringRep(deserialized[i]),$"{lineName}[{i}]");
            }
        }
        private string getStringRep(TileModel t){
            if(t==null) return "<null>";
            return $"{t.Id};{t.Type}";
        }
    }
}

