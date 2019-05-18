using System;
using System.Collections.Generic;
namespace Models.Hashing
{
    public static class ModelHashUtils 
    {
        public static void CombineHash(ref int hash,int? hash2){
            if(hash2!=null)
            unchecked{ 
                hash=hash*31+hash2.Value;
            }
        }
        public static int HashOfSet<THashable>(ISet<THashable> toHash){
            int hash=17;
            int multFactor=-1640531527;
            if(toHash!=null)
            unchecked{
                hash=19;//hash different for null vs empty
                foreach(var hashable in toHash){
                    hash+=(hashable?.GetHashCode()??0)*multFactor;
                }
            }
            return hash;
        }
        public static int HashOfEnumerable(IEnumerable<bool> toHash)
        {
            int hash=0;
            if(toHash!=null)
            unchecked{
                var enumerator=toHash.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    hash=hash<<1|(enumerator.Current?1:0);
                }
            }
            return hash;
        }
        public static int HashOfEnumerable<THashable>(IEnumerable<THashable> toHash) 
        {
            int hash = 17;
            if(toHash!=null)
            unchecked{
                hash=19;//hash different for null vs empty
                var enumerator=toHash.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    if(enumerator.Current!=null)
                    hash=hash*31+enumerator.Current.GetHashCode();
                }
            }
            return hash;
        }
        public static int HashOfEnumerableOfEnumerable<THashable>(IEnumerable<IEnumerable<THashable>> toHash)
        {
            int hash = 17;
            if(toHash!=null)
            unchecked{
                hash=19;//hash different for null vs empty
                var enumerator=toHash.GetEnumerator();
                while(enumerator.MoveNext())
                {
                    if(enumerator.Current!=null)
                    hash=hash*31+HashOfEnumerable(enumerator.Current);
                }
            }
            return hash;
        }
    }
}
