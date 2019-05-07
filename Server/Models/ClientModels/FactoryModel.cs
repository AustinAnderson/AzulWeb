using System;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class FactoryModel:IDeepCopyable<FactoryModel>
    {
        public bool IsEmpty=>  TileOne==null&&TileTwo==null&&
                             TileThree==null&&TileFour==null;
        public TileModel TileOne {get;set;}
        public TileModel TileTwo {get;set;}
        public TileModel TileThree {get;set;}
        public TileModel TileFour {get;set;}
        public int IndexLimit = 4;
        public TileModel this[int key]{
            get{
                if(key<0) throw new ArgumentOutOfRangeException("index must be positive and less than 4");
                switch(key) {
                    case 1: return TileOne;
                    case 2: return TileTwo;
                    case 3: return TileThree;
                    case 4: return TileFour;
                   default: throw new ArgumentOutOfRangeException("there are only 4 patternLines");
                }
            }
        }

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

