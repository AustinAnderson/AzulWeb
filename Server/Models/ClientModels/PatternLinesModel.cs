using System;
using Models.Hashing;

namespace Models.Client
{
    public class PatternLinesModel
    {
        public TileModel[] LineOne {get;set;} = new TileModel[1];
        public TileModel[] LineTwo {get;set;} = new TileModel[2];
        public TileModel[] LineThree {get;set;} = new TileModel[3];
        public TileModel[] LineFour {get;set;} = new TileModel[4];
        public TileModel[] LineFive {get;set;} = new TileModel[5];
        public TileModel[] this[int key]{
            get{
                if(key<0) throw new ArgumentOutOfRangeException("index must be positive and less than 5");
                switch(key) {
                    case 1: return LineOne;
                    case 2: return LineTwo;
                    case 3: return LineThree;
                    case 4: return LineFour;
                    case 5: return LineFive;
                   default: throw new ArgumentOutOfRangeException("there are only 5 patternLines");
                }
            }
        }
        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineOne));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineTwo));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineThree));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineFour));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineFive));
            return hash;
        }
    }
}

