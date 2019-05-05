using System;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class FactoryModel:IDeepCopyable<FactoryModel>
    {
        public TileModel TileOne {get;set;}
        public TileModel TileTwo {get;set;}
        public TileModel TileThree {get;set;}
        public TileModel TileFour {get;set;}

        public FactoryModel DeepCopy()
        {
            return new FactoryModel
            {
                TileOne=TileOne?.DeepCopy(),
                TileTwo=TileTwo?.DeepCopy(),
                TileThree=TileThree?.DeepCopy(),
                TileFour=TileFour?.DeepCopy(),
            };
        }

        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,TileOne.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,TileTwo.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,TileThree.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,TileFour.GetHashCode());
            return hash;
        }
    }
}

