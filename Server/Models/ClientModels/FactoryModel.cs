using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.ClientModels;
using Server.Models.Cloning;

namespace Models.Client
{
    public class FactoryModel:AbstractIndexedModel<TileModel>
    {
        public TileModel TileOne {get=>this[0];set=>this[0]=value;}
        public TileModel TileTwo {get=>this[1];set=>this[1]=value;}
        public TileModel TileThree {get=>this[2];set=>this[2]=value;}
        public TileModel TileFour {get=>this[3];set=>this[3]=value;}

        public bool IsEmpty=>  TileOne==null&&TileTwo==null&&
                             TileThree==null&&TileFour==null;
        protected override Dictionary<int, string> GetIndexedNames()
            =>new Dictionary<int, string>{
                {0,nameof(TileOne)},
                {1,nameof(TileTwo)},
                {2,nameof(TileThree)},
                {3,nameof(TileFour)}
            };

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

