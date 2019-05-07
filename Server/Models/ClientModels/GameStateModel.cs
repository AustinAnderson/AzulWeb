using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class GameStateModel:IDeepCopyable<GameStateModel>
    {
        public SharedDataModel SharedData {get;set;}
        public List<PlayerDataModel> PlayerData {get;set;}

        public int CurrentPlayersTurn{get;set;}


        public override int GetHashCode()
        {
            int hash = 17;
            ModelHashUtils.CombineHash(ref hash,SharedData?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(PlayerData));
            ModelHashUtils.CombineHash(ref hash,CurrentPlayersTurn);
            return hash;
        }
        public GameStateModel DeepCopy()
        {
            return new GameStateModel
            {
                SharedData=SharedData.DeepCopy(),
                PlayerData=DeepCopyObj<PlayerDataModel>.List(PlayerData)
                CurrentPlayersTurn=CurrentPlayersTurn
            };
        }
    }
}
