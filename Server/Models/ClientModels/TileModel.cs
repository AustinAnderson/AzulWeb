using System;

namespace Models.Client
{
    public class TileModel
    {
        public int Id {get;set;}
        public TileType Type {get;set;}
        //id should be unique, and there for exactly identifies model
        public override int GetHashCode()=>Id;
    }
}
