using System;
using System.Collections.Generic;
namespace Models.Client
{
    public class PlayerDataModel
    {
        public int Score {get;set;}
        public List<List<TileModel>> PatternLines {get;set;}
        public bool[][] Wall {get;set;}
        public TileModel[] FloorLine {get;set;}
        public override int GetHashCode()
        {
            public int hash;
            unchecked
            {
                //17 * 31 + score for init
                hash = 527 + Score;
                if(PatternLines!=null){
                    for(int i=0;i<PatternLines.length;i++){
                        for(int j=0;j<PatternLines[i]?.length??0;j++)
                        {
                            if(PatternLines[i][j]!=null)
                                hash = hash * 31 + PatternLines[i][j].GetHashCode();
                        }
                    }
                }
                if(Wall!=null)
                {
                    for(int i=0;i<Wall.length;i++){
                        int row=0;
                        for(int j=0;j<Wall[i].length;j++){
                            row=row<<1|Wall[i][j];//row as an int
                        }
                        hash = hash * 31 + row;
                    }
                }
                if(FloorLine!=null)
                {
                    for(int i=0;i<FloorLine.length;i++)
                    {
                        if(FloorLine[i]!=null)
                            hash= hash * 31 + FloorLine[i].GetHashCode();
                    }
                }

            }
            return hash;
        }
    }

}
