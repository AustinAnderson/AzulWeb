using System;
using System.Collections;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.ClientModels;
using Server.Models.Cloning;

namespace Models.Client
{
    //for serialization purposes, easier to use straight array and extensions
    //than custom class
    public static class PatternLineExtensions
    {
        public static bool IsEmpty(this TileModel[] tileList) {
            bool empty=true;
            for(int j=0;j<tileList.Length;j++){
                if(tileList[j]!=null) empty=false;
            }
            return empty;
        }
    }
    public class PatternLinesModel:AbstractIndexedModel<TileModel[]>,IDeepCopyable<PatternLinesModel>
    {
        public PatternLinesModel(){
            LineOne=new TileModel[1];
            LineTwo=new TileModel[2];
            LineThree=new TileModel[3];
            LineFour=new TileModel[4];
            LineFive=new TileModel[5];
        }

        //these should fill from left to right, client can display right to left if they want 
        public TileModel[] LineOne {get=>this[0];set=>this[0]=value;}
        public TileModel[] LineTwo {get=>this[1];set=>this[1]=value;}
        public TileModel[] LineThree {get=>this[2];set=>this[2]=value;}
        public TileModel[] LineFour {get=>this[3];set=>this[3]=value;}
        public TileModel[] LineFive {get=>this[4];set=>this[4]=value;}
        protected override Dictionary<int, string> GetIndexedNames()
            =>new Dictionary<int, string>{
                {0,nameof(LineOne)},
                {1,nameof(LineTwo)},
                {2,nameof(LineThree)},
                {3,nameof(LineFour)},
                {4,nameof(LineFive)}
            };

        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(LineOne));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(LineTwo));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(LineThree));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(LineFour));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(LineFive));
            return hash;
        }

        public PatternLinesModel DeepCopy() => new PatternLinesModel{
            LineOne=DeepCopyObj<TileModel>.Array(this.LineOne),
            LineTwo=DeepCopyObj<TileModel>.Array(this.LineTwo),
            LineThree=DeepCopyObj<TileModel>.Array(this.LineThree),
            LineFour=DeepCopyObj<TileModel>.Array(this.LineFour),
            LineFive=DeepCopyObj<TileModel>.Array(this.LineFive)
        };
    }
}

