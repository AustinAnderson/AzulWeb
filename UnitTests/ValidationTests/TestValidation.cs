using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;
using Server;
using Server.Logic;

namespace UnitTests.ValidationTests
{
    [TestClass]
    public class TestValidation
    {
        const string anyStr=".*?";
        [TestMethod]
        public void CatchClientServerMismatch(){
            ClientActionValidator validator=new ClientActionValidator();
            GameStateModel model=GetBaseGameStateModel();
            validator.ValidateRequest(new ClientRequestModel{
                
            })
        }
        [TestMethod]
        public void CatchClientChoseEmptyFactory(){
            ClientActionValidator validator=new ClientActionValidator();
        }
        [TestMethod]
        public void CatchClientChoseWrongTypeFromFactory(){
            ClientActionValidator validator=new ClientActionValidator();
        }
        [TestMethod]
        public void CatchClientChoseWrongTypeFromCenterOfTable(){
            ClientActionValidator validator=new ClientActionValidator();
        }
        [TestMethod]
        public void CatchClientChoseFromEmptyCenterOfTable(){
            ClientActionValidator validator=new ClientActionValidator();
        }
        [TestMethod]
        public void CatchClientChoseMismatchedPatternLine(){
            ClientActionValidator validator=new ClientActionValidator();
        }
        [TestMethod]
        public void CatchClientChoseFullPatternLine(){
            ClientActionValidator validator=new ClientActionValidator();
        }
        private GameStateModel GetBaseGameStateModel(){
            return new GameStateModel{
                SharedData=new SharedDataModel{
                    CenterOfTable=new List<TileModel>{
                        {new TileModel{Id=1,Type=TileType.Black}}
                    },
                    Factories=new FactoryModel[]{
                        new FactoryModel{
                            TileThree=new TileModel{Id=2,Type=TileType.Blue},
                            TileFour=new TileModel{Id=3,Type=TileType.Red}
                        },
                        new FactoryModel{
                            TileTwo=new TileModel{Id=4,Type=TileType.White}
                        }
                    },
                    Bag=new List<TileModel>{new TileModel{Id=1,}}
                },
                PlayerData=new List<PlayerDataModel>{
                    new PlayerDataModel{
                        Score=1,
                        PatternLines=new PatternLinesModel(){
                            LineOne=new TileModel[]{new TileModel{Id=7,Type=TileType.Blue}}
                        },
                        Wall=new TileModel[][]{
                            new TileModel[]{null,null,null,null,null},
                            new TileModel[]{null,null,null,null,null},
                            new TileModel[]{null,null,null,null,null},
                            new TileModel[]{null,null,null,null,null},
                            new TileModel[]{null,null,null,null,null}
                        },
                        FloorLine=new TileModel[]{
                            new TileModel{Id=0,Type=TileType.FirstPlayerMarker},
                            null,null,null,null,null,null}
                    }
                }
            };
        }
    }
}

