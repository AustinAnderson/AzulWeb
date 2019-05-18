using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class PlayerDataModel:IDeepCopyable<PlayerDataModel>
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
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(FloorLine));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerableOfEnumerable(Wall));
            return hash;
        }
        public PlayerDataModel DeepCopy()=>new PlayerDataModel{
            Score=this.Score,
            PatternLines=this.PatternLines?.DeepCopy(),
            Wall=DeepCopyObj<TileModel>.ArrayOfArray(this.Wall),
            FloorLine=DeepCopyObj<TileModel>.Array(this.FloorLine)
        };
    }

}
