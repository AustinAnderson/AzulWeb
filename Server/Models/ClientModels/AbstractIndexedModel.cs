using System;
using System.Collections.Generic;

namespace Server.Models.ClientModels
{
    public abstract class AbstractIndexedModel<TDictType,TReturnType>
    {
        private Dictionary<int,(TDictType,string)> indexedProps;
        protected abstract Dictionary<int,(TDictType,string)> GetIndexedProps();
        private (TDictType,string) GetOrThrow(int ndx){
            if(indexedProps==null) indexedProps=GetIndexedProps();
            if(ndx<0) throw new ArgumentOutOfRangeException("index must be positive");
            if(indexedProps.TryGetValue(ndx,out (TDictType,string) val)){
                return val;
            }
            throw new ArgumentOutOfRangeException(
                $"index must be less than {nameof(IndexLimit)} of {IndexLimit}"
            );
        }
        protected abstract TReturnType ReturnFromDict(TDictType type);
        public abstract int IndexLimit {get;}
        public string NameOf(int ndx)=>GetOrThrow(ndx).Item2;
        public TReturnType this[int ndx] {
            get=>ReturnFromDict(GetOrThrow(ndx).Item1);
            set{
            }
        }
    }
}

