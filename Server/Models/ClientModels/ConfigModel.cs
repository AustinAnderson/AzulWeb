using System;
using Models.Hashing;

namespace Models.Client
{
    public class ConfigModel
    {
        public int[] FloorPenalties {get;set;}
        public TileType[][] WallLayoutToMatch {get;set;}
        public override int GetHashCode(){
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(FloorPenalties));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashListOfList(WallLayoutToMatch));
            return hash;
        }
    }
}
