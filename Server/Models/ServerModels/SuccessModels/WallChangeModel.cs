using System;
namespace Models.Server
{
    public class WallChangeModel
    {
        public int PlayerIndex {get;set;}
        public int TileId {get;set;}
        public int RowIndex {get;set;}
        public int ColIndex {get;set;}
    }
}
