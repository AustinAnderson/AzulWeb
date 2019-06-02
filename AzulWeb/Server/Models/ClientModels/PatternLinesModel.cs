using System;
using System.Collections;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.ClientModels;
using Server.Models.Cloning;

namespace Models.Client
{
    public class PatternLinesModel:AbstractIndexedModel<FixedLengthTileModelQueue>,IDeepCopyable<PatternLinesModel>
    {
        public PatternLinesModel(){
            LineOne=new FixedLengthTileModelQueue(1);
            LineTwo=new FixedLengthTileModelQueue(2);
            LineThree=new FixedLengthTileModelQueue(3);
            LineFour=new FixedLengthTileModelQueue(4);
            LineFive=new FixedLengthTileModelQueue(5);
        }

        //these should fill from left to right, client can display right to left if they want 
        public FixedLengthTileModelQueue LineOne {get=>this[0];set=>this[0]=value;}
        public FixedLengthTileModelQueue LineTwo {get=>this[1];set=>this[1]=value;}
        public FixedLengthTileModelQueue LineThree {get=>this[2];set=>this[2]=value;}
        public FixedLengthTileModelQueue LineFour {get=>this[3];set=>this[3]=value;}
        public FixedLengthTileModelQueue LineFive {get=>this[4];set=>this[4]=value;}
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
            LineOne=LineOne?.DeepCopy(),
            LineTwo= LineTwo?.DeepCopy(),
            LineThree= LineThree?.DeepCopy(),
            LineFour= LineFour?.DeepCopy(),
            LineFive= LineFive?.DeepCopy()
        };
    }
}

