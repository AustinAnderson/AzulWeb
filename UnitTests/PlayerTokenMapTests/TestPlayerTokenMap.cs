using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Server.Logic;

namespace UnitTests.PlayerTokenMapTests
{
    [TestClass]
    public class TestPlayerTokenMap
    {
        [TestMethod]
        public void ModelSurvivesRoundTrip(){
            Dictionary<Guid,int> playerMap=getDict();
            Dictionary<Guid,int> roundTripMap=getDict();
            PlayerTokenMapHandler handler=new PlayerTokenMapHandler(Guid.NewGuid(),Guid.NewGuid());
            roundTripMap=handler.DecryptMap(handler.EncryptedMap(roundTripMap));
            AssertDictionariesSame(playerMap,roundTripMap);
        }
        private static Guid[] FixedGuidList=new Guid[6]{
            Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid()
        };
        private Dictionary<Guid,int> getDict()=>new Dictionary<Guid, int>{
            {FixedGuidList[0],1},
            {FixedGuidList[1],5},
            {FixedGuidList[2],2},
            {FixedGuidList[3],3},
            {FixedGuidList[4],8},
            {FixedGuidList[5],9},
        };
        private void AssertDictionariesSame(Dictionary<Guid,int> a,Dictionary<Guid,int> b){
            foreach(var key in a.Keys){
                if(b.TryGetValue(key,out int value))
                {
                    if(value!=a[key]) 
                        Assert.Fail($"values different for key {key}; a:{a[key]} b:{value}");
                }else{
                    Assert.Fail($"a has key {key} which b doesn't");
                }
            }
            foreach(var key in b.Keys){
                if(!a.ContainsKey(key)){
                    Assert.Fail($"b has key {key} which a doesn't");
                }
            }
        }
    }
}

