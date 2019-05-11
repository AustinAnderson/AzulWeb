using System;
using System.Collections.Generic;
using Models.Hashing;

namespace Models.Client
{
    public class PatternLinesModel
    {
        private readonly Dictionary<int,(TileModel[] line,string name)> indexer;
        public PatternLinesModel(){
            indexer=new Dictionary<int, (TileModel[] line, string name)>{
                {0,(LineOne,nameof(LineOne))},
                {1,(LineTwo,nameof(LineTwo))},
                {2,(LineThree,nameof(LineThree))},
                {3,(LineFour,nameof(LineFour))},
                {4,(LineFive,nameof(LineFive))}
            };
        }
        //these should fill from left to right, client can display right to left if they want 
        public TileModel[] LineOne {get;set;} = new TileModel[1];
        public TileModel[] LineTwo {get;set;} = new TileModel[2];
        public TileModel[] LineThree {get;set;} = new TileModel[3];
        public TileModel[] LineFour {get;set;} = new TileModel[4];
        public TileModel[] LineFive {get;set;} = new TileModel[5];
        private (TileModel[] line,string name) GetOrThrow(int ndx){
            if(ndx<0) throw new ArgumentOutOfRangeException("index must be positive and less than 5");
            if(indexer.TryGetValue(ndx,out (TileModel[],string) val)){
                return val;
            }
            throw new ArgumentOutOfRangeException("there are only 5 patternLines");
        }
        public TileModel[] this[int key]{
            get=>GetOrThrow(key).line;
        }
        public string NameOf(int patternLineIndex)
            =>GetOrThrow(patternLineIndex).name;
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

