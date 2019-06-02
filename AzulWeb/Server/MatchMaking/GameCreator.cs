using System;
using System.Collections.Generic;
using System.Linq;
using Models.Client;
using Server.Logic;
using Server.Models.ServerModels.SetupModels;

namespace Server.MatchMaking
{
    public class GameCreator
    {
        private static Random rng=new Random(DateTime.Now.GetHashCode());
        private static string symbolList=
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private PlayerTokenMapHandler playerTokenHandler;
        public GameCreator(PlayerTokenMapHandler playerTokenHandler){
            this.playerTokenHandler=playerTokenHandler;
        }
        public string GenerateNewGameCode()
            => ""+
               symbolList[rng.Next(0,symbolList.Length)]+
               symbolList[rng.Next(0,symbolList.Length)]+
               symbolList[rng.Next(0,symbolList.Length)]+
               symbolList[rng.Next(0,symbolList.Length)];

        public InitialStateModel GenerateNewGame(
            out Dictionary<string,PlayerIndexAndToken> connectionIdToConfidentialDataMap,
            string gameId, List<string> connectionIds
        )
        {
            GameStateModel newGame=new GameStateModel
            {
                GameId=gameId,
                SharedData=new SharedDataModel(),
                PlayerData=connectionIds.Select(connId=>new PlayerDataModel(connId)).ToList(),
            };
            newGame.SharedData.Factories=new FactoryModel[connectionIds.Count*2+1];
            for(int i=0;i<newGame.SharedData.Factories.Length;i++)
            {
                newGame.SharedData.Factories[i]=new FactoryModel();
            }

            connectionIdToConfidentialDataMap=new Dictionary<string, PlayerIndexAndToken>();
            Dictionary<Guid,int> tokensToPlayerIndexMap=new Dictionary<Guid, int>();
            for(int i=0;i<connectionIds.Count;i++)
            {
                var playerData=new PlayerIndexAndToken{
                    PlayerIndex=i,
                    PlayerToken=Guid.NewGuid()
                };
                connectionIdToConfidentialDataMap.Add(connectionIds[i],playerData);
                tokensToPlayerIndexMap.Add(playerData.PlayerToken,playerData.PlayerIndex);
            }
            newGame.PlayerTokenMapEnc=playerTokenHandler.EncryptedMap(tokensToPlayerIndexMap);

            return new InitialStateModel
            {
                InitialState=newGame,
                InitialHash=newGame.GetHashCode()
            };
        }
    }
}

