using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Client;
using Server;
using Server.Exceptions;
using Server.Logic;

namespace UnitTests.ValidationTests
{
    [TestClass]
    public class TestValidation
    {
        const string anyStr=".*?";
        public static readonly PlayerTokenMapHandler mapHandler=new PlayerTokenMapHandler(Guid.NewGuid(),Guid.NewGuid());
        private static readonly Guid playerOne=Guid.NewGuid();
        private static readonly Guid playerTwo=Guid.NewGuid();
        [TestMethod]
        public void CatchClientWithGarbageToken(){
            ClientActionValidator validator=new ClientActionValidator(mapHandler);
            ClientRequestModel model=GetBaseRequestModel();
            model.Action=new ActionDescriptionModel
            {
                PlayerToken=Guid.NewGuid(),
                FromFactory=true,
                FactoryIndex=0,
                TileType=TileType.Red,
                PatternLineIndex=1
            };
            model.GameStateHash=model.GameState.GetHashCode();
            try{
                validator.ValidateRequest(model);
                Assert.Fail("expected exception");
            }catch(Exception ex)
            {
                if(! (ex is BadRequestException)){
                    Assert.Fail("expected exception to be BadRequestException");
                }
                else{
                    AssertExpectedMessage(
                        ClientFacingMessages.BadRequestMessages.PlayerTokenInvalid(anyStr),
                        ex.Message
                    );
                }
            }
        }
        [TestMethod]
        public void CatchClientGoesWhenNotItsTurn(){
            ClientActionValidator validator=new ClientActionValidator(mapHandler);
            ClientRequestModel model=GetBaseRequestModel();
            model.Action=new ActionDescriptionModel
            {
                PlayerToken=playerTwo,
                FromFactory=true,
                FactoryIndex=0,
                TileType=TileType.Red,
                PatternLineIndex=1
            };
            model.GameStateHash=model.GameState.GetHashCode();
            try{
                validator.ValidateRequest(model);
                Assert.Fail("expected exception");
            }catch(Exception ex)
            {
                if(! (ex is BadRequestException)){
                    Assert.Fail("expected exception to be BadRequestException");
                }
                else{
                    AssertExpectedMessage(
                        ClientFacingMessages.BadRequestMessages.PlayerPlayedOutOfTurn(anyStr,anyStr),
                        ex.Message
                    );
                }
            }
        }
        [TestMethod]
        public void CatchClientServerMismatch(){
            ClientActionValidator validator=new ClientActionValidator(mapHandler);
            ClientRequestModel model=GetBaseRequestModel();
            model.Action=new ActionDescriptionModel
            {
                PlayerToken=playerOne,
                FromFactory=true,
                FactoryIndex=0,
                TileType=TileType.Red,
                PatternLineIndex=1
            };
            model.GameStateHash=model.GameState.GetHashCode()+1;
            try{
                validator.ValidateRequest(model);
                Assert.Fail("expected exception");
            }catch(Exception ex)
            {
                if(! (ex is BadRequestException)){
                    Assert.Fail("expected to be BadRequestException");
                }else
                {
                    AssertExpectedMessage(
                        ClientFacingMessages.BadRequestMessages.ClientServerMismatchHash(anyStr,anyStr),
                        ex.Message
                    );
                }
            }
        }
        [TestMethod]
        public void CatchClientChoseEmptyFactory(){
            ClientActionValidator validator=new ClientActionValidator(mapHandler);
            var model=GetBaseRequestModel();
            model.Action=new ActionDescriptionModel
            {
                PlayerToken=playerOne,
                PatternLineIndex=1,
                TileType=TileType.Black,
                FactoryIndex=2,
                FromFactory=true
            };
            model.GameStateHash=model.GameState.GetHashCode();
            var response=validator.ValidateRequest(model);
            Assert.IsNotNull(response,"expected validation to flag an error");
            Assert.AreEqual(false,response.ErrorOnCenterOfTable,"expected error model to indicate error was not from center of table");
            Assert.AreEqual(2,response.FactoryIndex,"expected error model to indicate which factory");
            AssertExpectedMessage(ClientFacingMessages.NoDrawingFromEmptyFactory(),response.Message);
        }
        [TestMethod]
        public void CatchClientChoseWrongTypeFromFactory(){
            ClientActionValidator validator=new ClientActionValidator(mapHandler);
            var model=GetBaseRequestModel();
            model.Action=new ActionDescriptionModel
            {
                PlayerToken=playerOne,
                PatternLineIndex=1,
                TileType=TileType.Black,
                FactoryIndex=1,
                FromFactory=true
            };
            model.GameStateHash=model.GameState.GetHashCode();
            var response=validator.ValidateRequest(model);
            Assert.IsNotNull(response,"expected validation to flag an error");
            Assert.AreEqual(false,response.ErrorOnCenterOfTable,"expected error model to indicate error was not from center of table");
            Assert.AreEqual(1,response.FactoryIndex,"expected error model to indicate which factory");
            AssertExpectedMessage(ClientFacingMessages.InvalidTileTypeForFactory(anyStr),response.Message);
        }
        [TestMethod]
        public void CatchClientChoseWrongTypeFromCenterOfTable(){
            ClientActionValidator validator=new ClientActionValidator(mapHandler);
            var model=GetBaseRequestModel();
            model.Action=new ActionDescriptionModel
            {
                PlayerToken=playerOne,
                PatternLineIndex=1,
                TileType=TileType.White,
                FromFactory=false
            };
            model.GameStateHash=model.GameState.GetHashCode();
            var response=validator.ValidateRequest(model);
            Assert.IsNotNull(response,"expected validation to flag an error");
            Assert.AreEqual(true,response.ErrorOnCenterOfTable,"expected error model to indicate error was from center of table");
            AssertExpectedMessage(ClientFacingMessages.InvalidTileTypeForCenterOfTable(anyStr),response.Message);
        }
        [TestMethod]
        public void CatchClientChoseFromEmptyCenterOfTable(){
            ClientActionValidator validator=new ClientActionValidator(mapHandler);
            var model=GetBaseRequestModel();
            model.GameState.SharedData.CenterOfTable.Clear();
            model.Action=new ActionDescriptionModel
            {
                PlayerToken=playerOne,
                PatternLineIndex=1,
                TileType=TileType.White,
                FromFactory=false
            };
            model.GameStateHash=model.GameState.GetHashCode();
            var response=validator.ValidateRequest(model);
            Assert.IsNotNull(response,"expected validation to flag an error");
            Assert.AreEqual(true,response.ErrorOnCenterOfTable,"expected error model to indicate error was from center of table");
            AssertExpectedMessage(ClientFacingMessages.NoDrawingFromEmptyCenterOfTable(),response.Message);
        }
        [TestMethod]
        public void CatchClientChoseMismatchedPatternLine(){
            ClientActionValidator validator=new ClientActionValidator(mapHandler);
            var model=GetBaseRequestModel();
            model.GameState.SharedData.CenterOfTable.Clear();
            model.Action=new ActionDescriptionModel
            {
                PlayerToken=playerOne,
                PatternLineIndex=2,
                FactoryIndex=0,
                TileType=TileType.Red,
                FromFactory=true
            };
            model.GameStateHash=model.GameState.GetHashCode();
            var response=validator.ValidateRequest(model);
            Assert.IsNotNull(response,"expected validation to flag an error");
            Assert.AreEqual(false,response.ErrorOnCenterOfTable,"expected error model to indicate error was not from center of table");
            AssertExpectedMessage(
                ClientFacingMessages.PatternLineContainsDifferentType(anyStr,anyStr,anyStr),
                response.Message
            );
        }
        private ClientRequestModel GetBaseRequestModel(){
            var dict=new Dictionary<Guid, int>{
                {playerOne,0},
                {playerTwo,1}
            };
            var model=new ClientRequestModel{

                GameState=new GameStateModel{
                    PlayerTokenMapEnc=mapHandler.EncryptedMap(dict),
                    SharedData=new SharedDataModel{
                        CurrentTurnsPlayersIndex=0,
                        CenterOfTable=new HashSet<TileModel>{
                            {new TileModel{Id=1,Type=TileType.Black}}
                        },
                        Factories=new FactoryModel[]{
                            new FactoryModel{
                                TileThree=new TileModel{Id=2,Type=TileType.Blue},
                                TileFour=new TileModel{Id=3,Type=TileType.Red}
                            },
                            new FactoryModel{
                                TileTwo=new TileModel{Id=4,Type=TileType.White}
                            },
                            new FactoryModel(),
                            new FactoryModel(),
                            new FactoryModel(),
                        },
                        Bag=new HashSet<TileModel>{new TileModel{Id=1,}}
                    },
                    PlayerData=new List<PlayerDataModel>{
                        new PlayerDataModel(),
                        new PlayerDataModel()
                    }
                }
            };
            model.GameState.PlayerData[0].PatternLines.LineOne[0]=new TileModel{Id=22,Type=TileType.White};
            model.GameState.PlayerData[0].PatternLines.LineThree[0]=new TileModel{Id=23,Type=TileType.Yellow};
            model.GameState.PlayerData[0].PatternLines.LineThree[1]=new TileModel{Id=24,Type=TileType.Yellow};
            return model;
        }
        private void AssertExpectedMessage(string expectedPattern,string actual){
            Regex matcher=new Regex(expectedPattern);
            Assert.IsTrue(matcher.IsMatch(actual),$"expected message to match {expectedPattern}");
        }
    }
}

