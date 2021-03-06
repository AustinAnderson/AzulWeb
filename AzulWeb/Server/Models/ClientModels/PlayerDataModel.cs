using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class PlayerDataModel:IDeepCopyable<PlayerDataModel>
    {
        public PlayerDataModel(){}
        public PlayerDataModel(string connectionId){
            ConnectionId=connectionId;
        }
        public int Score {get;set;}=0;
        public string ConnectionId{get;set;}
        public PatternLinesModel PatternLines {get;set;} =new PatternLinesModel();
        public TileModel[][] Wall {get;set;}=new TileModel[5][]{
            new TileModel[5],new TileModel[5],new TileModel[5],new TileModel[5],new TileModel[5]
        };
        public FixedLengthTileModelQueue FloorLine {get;set;}=new FixedLengthTileModelQueue(7);

        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,Score);
            ModelHashUtils.CombineHash(ref hash,ConnectionId?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,PatternLines.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(FloorLine));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerableOfEnumerable(Wall));
            return hash;
        }
        public PlayerDataModel DeepCopy()=>new PlayerDataModel{
            Score=this.Score,
            ConnectionId=this.ConnectionId,
            PatternLines=this.PatternLines?.DeepCopy(),
            Wall=DeepCopyObj<TileModel>.ArrayOfArray(this.Wall),
            FloorLine=FloorLine?.DeepCopy()
        };
    }
}
