using System;
using System.Collections.Generic;
namespace Models.Client
{
    public class GameStateModel
    {
        public SharedDataModel SharedData {get;set;}
        public List<PlayerDataModel> PlayerData {get;set;}
        public override int GetHashCode()
        {
            int hash = 17;
            unchecked
            {
                if(SharedData!=null) hash = hash * 31 + SharedData.GetHashCode();
                if(PlayerData!=null)
                {
                    for(int i=0;i<PlayerData.Count;i++){
                        if(PlayerData[i]!=null)
                        {
                            hash = hash * 31 + PlayerData[i].GetHashCode();
                        }
                    }
                }
            }
            return hash;
        }
    }
}
