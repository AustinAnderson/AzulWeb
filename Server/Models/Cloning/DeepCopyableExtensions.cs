using System.Collections.Generic;

namespace Server.Models.Cloning
{
    public static class DeepCopyStruct<T> where T:struct
    {
        public static T[] Array(T[] toDeepCopy){
            if(toDeepCopy==null) return null;
            T[] copy=new T[toDeepCopy.Length];
            for(int i=0;i<toDeepCopy.Length;i++){
                copy[i]=toDeepCopy[i];
            }
            return copy;
        }
        public static T[][] ArrayOfArray(T[][] toDeepCopy)
        {
            if(toDeepCopy==null) return null;
            T[][] copy=new T[toDeepCopy.Length][];
            for(int i=0;i<toDeepCopy.Length;i++){
                copy[i]=DeepCopyStruct<T>.Array(toDeepCopy[i]);
            }
            return copy;
        }
        public static L List<L>(L toDeepCopy) where L:IList<T>,new()
        {
            L list=new L();
            foreach(var copyAble in toDeepCopy)
            {
                list.Add(copyAble);
            }
            return list;
        }
        public static LL ListOfList<LL,LT>(LL toDeepCopy) 
                            where LL:IList<LT>, new() 
                            where LT:IList<T>,new()
        {
            LL mat=new LL();
            foreach(LT copyableList in toDeepCopy)
            {
                mat.Add(DeepCopyStruct<T>.List(copyableList));
            }
            return mat;
        }
    }
    public static class DeepCopyObj<T> where T:IDeepCopyable<T>
    {
        public static T[] Array(T[] toDeepCopy)
        {
            T[] copy=new T[toDeepCopy.Length];
            for(int i=0;i<toDeepCopy.Length;i++){
                copy[i]=toDeepCopy[i].DeepCopy();
            }
            return copy;
        }
        public static T[][] ArrayOfArray(T[][] toDeepCopy)
        {
            T[][] mat=new T[toDeepCopy.Length][];
            for(int i=0;i<toDeepCopy.Length;i++){
                mat[i]=DeepCopyObj<T>.Array(toDeepCopy[i]);
            }
            return mat;
        }
        public static L List<L>(L toDeepCopy) where L:IList<T>,new()
        {
            L list=new L();
            foreach(var copyAble in toDeepCopy)
            {
                list.Add(copyAble.DeepCopy());
            }
            return list;
        }
        public static LL ListOfList<LL,LT>(LL toDeepCopy) where LL:IList<LT>, new() where LT:IList<T>,new()
        {
            LL mat=new LL();
            foreach(LT copyableList in toDeepCopy)
            {
                mat.Add(DeepCopyObj<T>.List(copyableList));
            }
            return mat;
        }
    }
}
