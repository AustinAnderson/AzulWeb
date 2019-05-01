using System;
namespace Models.Client
{
    public class ConfigModel
    {
        public int[] FloorPenalties {get;set;}
        public TileType[][] WallLayoutToMatch {get;set;}
        public override int GetHashCode(){
            int hash=17;
            if(WallLayoutToMatch!=null)
            {
                unchecked
                {
                    for(int i=0;i<WallLayoutToMatch.Length;i++){
                        for(int j=0;j<WallLayoutToMatch[i].Length;j++){
                            hash=hash*31+((int)WallLayoutToMatch[i][j]);
                        }
                    }
                }
            }
            if(FloorPenalties!=null)
            {
                unchecked
                {
                    for(int i=0;i<FloorPenalties.Length;i++){
                        hash=hash*31+FloorPenalties[i];
                    }
                }
            }
            return hash;
        }
    }
}
