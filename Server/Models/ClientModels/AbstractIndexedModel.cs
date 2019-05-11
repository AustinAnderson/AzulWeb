using System;
using System.Collections.Generic;

namespace Server.Models.ClientModels
{
    ///adds an indexer and name to models that implement it, as long as any properties
    ///are done using the indexer
    public abstract class AbstractIndexedModel<T> where T:new()
    {
        private class Ref {
            public T item;
            public Ref(T t) => item=t;
        }
        private Dictionary<int,(Ref,string)> indexedProps;
        private void ConvertAndFillDict(Dictionary<int,string> childDict){
            indexedProps=new Dictionary<int, (Ref, string)>();
            foreach(var kvp in childDict){
                indexedProps.Add(kvp.Key,(new Ref(new T()),kvp.Value));
            }
        }
        private (Ref,string) GetOrThrow(int ndx){
            if(indexedProps==null) ConvertAndFillDict(GetIndexedNames());
            if(ndx<0) throw new ArgumentOutOfRangeException("index must be positive");
            if(indexedProps.TryGetValue(ndx,out (Ref,string) val)){
                return val;
            }
            throw new ArgumentOutOfRangeException(
                $"index must be less than {nameof(IndexLimit)} of {IndexLimit}"
            );
        }
        protected abstract Dictionary<int,string> GetIndexedNames();
        public abstract int IndexLimit {get;}
        public string NameOf(int ndx)=>GetOrThrow(ndx).Item2;
        public T this[int ndx] {
            get=>GetOrThrow(ndx).Item1.item;
            set{
                GetOrThrow(ndx).Item1.item=value;
            }
        }
    }
}

