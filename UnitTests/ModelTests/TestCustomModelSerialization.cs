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
        }
    }
}

