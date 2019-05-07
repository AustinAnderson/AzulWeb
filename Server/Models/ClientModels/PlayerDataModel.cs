using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class PlayerDataModel
    {
        public int Score {get;set;}=0;
        public PatternLinesModel PatternLines {get;set;} =new PatternLinesModel();
        public TileModel[][] Wall {get;set;}=new TileModel[5][]{
            new TileModel[5],new TileModel[5],new TileModel[5],new TileModel[5],new TileModel[5]
        };
        public TileModel[] FloorLine {get;set;}=new TileModel[7];

        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,Score);
            ModelHashUtils.CombineHash(ref hash,PatternLines.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(FloorLine));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashListOfList(Wall));
            return hash;
        }
    }

}
