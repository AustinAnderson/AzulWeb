using System;
using Server.Models.Cloning;

namespace Models.Client
{
    public class TileModel
    {
        public TileModel(){}
        public TileModel(int id,TileType type){
            Id=id;
            Type=type;
        }
        public int Id {get;set;}
        public TileType Type {get;set;}

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return (obj as TileModel).Id==Id;
        }
        
        //id should be unique, and there for exactly identifies model
        public override int GetHashCode()=>Id;
    }
}
