using System;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class ConfigModel:IDeepCopyable<ConfigModel>
    {
        public int[] FloorPenalties {get;set;}
        public TileType[][] WallLayoutToMatch {get;set;}


        public override int GetHashCode(){
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(FloorPenalties));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerableOfEnumerable(WallLayoutToMatch));
            return hash;
        }
        public ConfigModel DeepCopy()=> new ConfigModel{
            FloorPenalties=DeepCopyStruct<int>.Array(this.FloorPenalties),
            WallLayoutToMatch=DeepCopyStruct<TileType>.ArrayOfArray(this.WallLayoutToMatch)
        };
    }
}
