using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;
using Newtonsoft.Json;

namespace UnitTests.ModelTests
{
    [TestClass]
    public class TestModelCopy
    {
        [TestMethod]
        public void TestCopyGameState(){
            GameStateModel model=GetModel();
            GameStateModel copy=model.DeepCopy();
            Assert.IsFalse(model==copy,"reference equality should be false between original and copy");
            Assert.AreEqual(
                JsonConvert.SerializeObject(model),
                JsonConvert.SerializeObject(copy),
                "should serialize the same"
            );
        }
        private GameStateModel GetModel(){
            int i=0;
            var model=new GameStateModel{
                PlayerTokenMapEnc="some string",
                SharedData=new SharedDataModel{
                    CurrentTurnsPlayersIndex=3,
                    DiscardPile=new List<TileModel>{new TileModel(i++,TileType.Blue)},
                    Bag=new List<TileModel>{new TileModel(i++,TileType.Red)},
                    CenterOfTable=new List<TileModel>{new TileModel(i++,TileType.Red),new TileModel(i++,TileType.FirstPlayerMarker)},
                    Factories=new FactoryModel[]{
                        new FactoryModel{
                            TileOne=new TileModel(i++,TileType.White),TileThree=new TileModel(i++,TileType.Red)
                        },
                        new FactoryModel{
                            TileFour=new TileModel(i++,TileType.White),TileThree=new TileModel(i++,TileType.Red)
                        },
                    },
                    Config=new ConfigModel{
                        FloorPenalties=new int[]{3,2,5},
                        WallLayoutToMatch=new TileType[][]{
                            new TileType[]{TileType.Red,TileType.Black,TileType.Blue},
                            new TileType[]{TileType.Red,TileType.Black,TileType.Blue},
                            new TileType[]{TileType.Red,TileType.White,TileType.Blue},
                            new TileType[]{TileType.Red,TileType.Black,TileType.Yellow}
                        }
                    }
                },
                PlayerData=new List<PlayerDataModel>(){
                    new PlayerDataModel{
                        Score=3,
                        PatternLines=new PatternLinesModel{
                            LineTwo=new TileModel[]{new TileModel(i++,TileType.Yellow),null}
                        }
                    },
                    new PlayerDataModel{
                        PatternLines=new PatternLinesModel()
                    }
                }
            };
            model.PlayerData[0].PatternLines[4][2]=new TileModel(i++,TileType.Black);
            model.PlayerData[0].FloorLine[2]=new TileModel(i++,TileType.Black);
            model.PlayerData[0].Wall[0][2]=new TileModel(i++,TileType.White);
            return model;
        }
    }
}

