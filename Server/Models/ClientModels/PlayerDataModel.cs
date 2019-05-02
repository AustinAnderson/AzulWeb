using System;
using System.Collections.Generic;
using Models.Hashing;

namespace Models.Client
{
    public class PlayerDataModel
    {
        public int Score {get;set;}
        public List<List<TileModel>> PatternLines {get;set;}
        public bool[][] Wall {get;set;}
        public TileModel[] FloorLine {get;set;}

        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,Score);
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashListOfList(PatternLines));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(FloorLine));
            if(Wall!=null)
            {
                for(int i=0;i<Wall.Length;i++)
                {
                    ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashBoolList(Wall[i]));
                }
            }
            return hash;
        }
    }

}
