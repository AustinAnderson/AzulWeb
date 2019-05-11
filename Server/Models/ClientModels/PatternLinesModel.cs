using System;
using Models.Hashing;

namespace Models.Client
{
    public class PatternLinesModel
    {
        //these should fill from left to right, client can display right to left if they want 
        public TileModel[] LineOne {get;set;} = new TileModel[1];
        public TileModel[] LineTwo {get;set;} = new TileModel[2];
        public TileModel[] LineThree {get;set;} = new TileModel[3];
        public TileModel[] LineFour {get;set;} = new TileModel[4];
        public TileModel[] LineFive {get;set;} = new TileModel[5];
        public TileModel[] this[int key]{
            get{
                if(key<0) throw new ArgumentOutOfRangeException("index must be positive and less than 5");
                switch(key) {
                    case 0: return LineOne;
                    case 1: return LineTwo;
                    case 2: return LineThree;
                    case 3: return LineFour;
                    case 4: return LineFive;
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

