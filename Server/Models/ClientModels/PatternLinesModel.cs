using System;
namespace Models.Client
{
    public class PatternLinesModel
    {
        public TileModel[] LineOne {get;set;} = new TileModel[1];
        public TileModel[] LineTwo {get;set;} = new TileModel[2];
        public TileModel[] LineThree {get;set;} = new TileModel[3];
        public TileModel[] LineFour {get;set;} = new TileModel[4];
        public TileModel[] LineFive {get;set;} = new TileModel[5];
        public override int GetHashCode()
        {
            
        }
    }
}

