using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class FactoryModel
    {
        public TileModel TileOne {get;set;}
        public TileModel TileTwo {get;set;}
        public TileModel TileThree {get;set;}
        public TileModel TileFour {get;set;}

        //use reference class wrapper to allow setting tiles to null;
        private class TileRef{public TileRef(TileModel m)=>tile=m;public TileModel tile;}
        private readonly Dictionary<int,(TileRef,string)> indexer;
        public FactoryModel(){
            indexer=new Dictionary<int, (TileRef, string)>{
                {0,(new TileRef(TileOne),nameof(TileOne))},
                {1,(new TileRef(TileTwo),nameof(TileTwo))},
                {2,(new TileRef(TileThree),nameof(TileThree))},
                {3,(new TileRef(TileFour),nameof(TileFour))}
            };
        }
        public bool IsEmpty=>  TileOne==null&&TileTwo==null&&
                             TileThree==null&&TileFour==null;
        public int IndexLimit = 4;
        private (TileRef tileRef,string name) GetOrThrow(int ndx){
            if(ndx<0) throw new ArgumentOutOfRangeException("index must be positive and less than 4");
            if(indexer.TryGetValue(ndx,out (TileRef,string) val)){
                return val;
            }
            throw new ArgumentOutOfRangeException("there are only 4 tiles in the factory");
        }
        public string NameOf(int ndx) => GetOrThrow(ndx).name;
        public TileModel this[int key]{
            get => GetOrThrow(key).tileRef.tile;
            set{
                GetOrThrow(key).tileRef.tile=value;
            }
        }
        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,TileOne?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,TileTwo?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,TileThree?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,TileFour?.GetHashCode());
            return hash;
        }
    }
}

