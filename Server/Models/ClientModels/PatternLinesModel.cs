using System;
using System.Collections;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.ClientModels;

namespace Models.Client
{
    public class PatternLine : IEnumerable<TileModel>
    {
        private readonly TileModel[] tileList;
        public PatternLine(int size){
            tileList=new TileModel[size];
        }
        public int Length=>tileList.Length;
        public TileModel this[int key]{
            get =>tileList[key];
            set => tileList[key]=value;
        }
        public bool IsEmpty {
            get{
                bool empty=true;
                for(int j=0;j<tileList.Length;j++){
                    if(tileList[j]!=null) empty=false;
                }
                return empty;
            }
        }
        public IEnumerator<TileModel> GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => tileList.GetEnumerator();
    }
    public class PatternLinesModel:AbstractIndexedModel<PatternLine>
    {
        public PatternLinesModel(){
            LineOne=new PatternLine(1);
            LineTwo=new PatternLine(2);
            LineThree=new PatternLine(3);
            LineFour=new PatternLine(4);
            LineFive=new PatternLine(5);
        }

        //these should fill from left to right, client can display right to left if they want 
        public PatternLine LineOne {get=>this[0];set=>this[0]=value;}
        public PatternLine LineTwo {get=>this[1];set=>this[1]=value;}
        public PatternLine LineThree {get=>this[2];set=>this[2]=value;}
        public PatternLine LineFour {get=>this[3];set=>this[3]=value;}
        public PatternLine LineFive {get=>this[4];set=>this[4]=value;}
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
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineOne));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineTwo));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineThree));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineFour));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(LineFive));
            return hash;
        }

    }
}

