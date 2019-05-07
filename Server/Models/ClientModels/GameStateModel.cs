using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class GameStateModel
    {
        public string PlayerTokenMapEnc {get;set;}
        public SharedDataModel SharedData {get;set;}
        public List<PlayerDataModel> PlayerData {get;set;}

        public override int GetHashCode()
        {
            int hash = 17;
            ModelHashUtils.CombineHash(ref hash,PlayerTokenMapEnc?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,SharedData?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(PlayerData));
            return hash;
        }
    }
}
