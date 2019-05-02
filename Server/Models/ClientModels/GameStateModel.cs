using System;
using System.Collections.Generic;
using Models.Hashing;

namespace Models.Client
{
    public class GameStateModel
    {
        public SharedDataModel SharedData {get;set;}
        public List<PlayerDataModel> PlayerData {get;set;}
        public override int GetHashCode()
        {
            int hash = 17;
            ModelHashUtils.CombineHash(ref hash,SharedData?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(PlayerData));
            return hash;
        }
    }
}
