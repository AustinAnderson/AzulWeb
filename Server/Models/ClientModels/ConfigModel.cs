using System;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class ConfigModel:IDeepCopyable<ConfigModel>
    {
        public int[] FloorPenalties {get;set;}
        public TileType[][] WallLayoutToMatch {get;set;}

        public ConfigModel DeepCopy()
        {
            return new ConfigModel{
                FloorPenalties=DeepCopyStruct<int>.Array(FloorPenalties),
                WallLayoutToMatch=DeepCopyStruct<TileType>.ArrayOfArray(WallLayoutToMatch)
            };
        }

        public override int GetHashCode(){
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(FloorPenalties));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashListOfList(WallLayoutToMatch));
            return hash;
        }
    }
}
