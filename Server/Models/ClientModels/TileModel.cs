using System;
using Server.Models.Cloning;

namespace Models.Client
{
    public class TileModel:IDeepCopyable<TileModel>
    {
        public int Id {get;set;}
        public TileType Type {get;set;}

        public TileModel DeepCopy()
        {
            return new TileModel {Id=Id,Type=Type};
        }
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
