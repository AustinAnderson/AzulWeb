using System;
using System.Collections.Generic;

namespace Server.Models.ClientModels
{
    public abstract class AbstractIndexedModel<T>
    {
        private Dictionary<int,(T,string)> indexedProps;
        protected abstract Dictionary<int,(T,string)> GetIndexedProps();
        public abstract int IndexLimit {get;}
        private (T,string) GetOrThrow(int ndx){
            if(indexedProps==null) indexedProps=GetIndexedProps();
            if(ndx<0) throw new ArgumentOutOfRangeException("index must be positive");
            if(indexedProps.TryGetValue(ndx,out (T,string) val)){
                return val;
            }
            throw new ArgumentOutOfRangeException(
                $"index must be less than {nameof(IndexLimit)} of {IndexLimit}"
            );
        }
        public string NameOf(int ndx)=>GetOrThrow(ndx).Item2;
        public T this[int ndx] {
            get
        }
    }
}

