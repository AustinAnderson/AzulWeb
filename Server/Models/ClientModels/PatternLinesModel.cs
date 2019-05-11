using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.ClientModels;

namespace Models.Client
{
    public class PatternLinesModel:AbstractIndexedModel<TileModel[]>
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
        public override int IndexLimit => 5;

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

