using System;
using Server.Models.Cloning;

namespace Models.Client
{
    public class TileModel:IDeepCopyable<TileModel>
    {
        private int id=0;
        private TileType type=TileType.Blank;
        public bool IsBlank => Id==0;
        public int Id {
            get=>id;
            set{
                if(value==0) type=TileType.Blank; 
                id=value;
            }
        }
        public TileType Type {
            get=>type;
            set{
                if(value==TileType.Blank) id=0;
                type=value;
            }
        }

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
