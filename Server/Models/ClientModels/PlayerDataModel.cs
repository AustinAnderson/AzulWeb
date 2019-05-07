using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class PlayerDataModel:IDeepCopyable<PlayerDataModel>
    {
        public int Score {get;set;}
        public PatternLinesModel PatternLines {get;set;}
        public TileModel[][] Wall {get;set;}
        public TileModel[] FloorLine {get;set;}

        public PlayerDataModel DeepCopy()
        {
            throw new NotImplementedException();
            return new PlayerDataModel
            {
                Score=Score,
                //TODO: do I need patternlines and and wall copying at all?
                FloorLine=DeepCopyObj<TileModel>.Array(FloorLine)
            };
        }

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
