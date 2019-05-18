using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Server.Models.Cloning;
using Server.Serialization;

namespace Models.Client
{
    [JsonConverter(typeof(FixedLengthTileModelQueueConverter))]
    public class FixedLengthTileModelQueue:IEnumerable<TileModel>,IDeepCopyable<FixedLengthTileModelQueue>
    {
        private int currentIndex;
        private TileModel[] tileList;
        public int Count => tileList.Length;

        public FixedLengthTileModelQueue(int length)
        {
            this.tileList = new TileModel[length];
            currentIndex=0;
        }
        public TileModel this[int ndx]{ get=>tileList[ndx]; }
        public bool IsEmpty => tileList[0]==null;

        public TileModel PopOrNull(){
            TileModel popped=null;
            if(currentIndex>=0){
                popped=tileList[currentIndex];
                tileList[currentIndex]=null;
                currentIndex--;
            }
            return popped;
        }
        ///<summary>returns true if the tile could be queued into the list, false if it was full</summary>
        public bool TryAdd(TileModel toAdd)
        {
            if(currentIndex>=tileList.Length) return false;
            tileList[currentIndex]=toAdd;
            currentIndex++;
            return true;
        }
        public List<TileModel> TryAddRange(IEnumerable<TileModel> tiles){
            List<TileModel> overflow=new List<TileModel>();
            foreach(var tile in tiles){
                if(!TryAdd(tile)){
                    overflow.Add(tile);
                }
            }
            return overflow;
        }

        public IEnumerator<TileModel> GetEnumerator()
        {
            foreach(var tile in tileList) yield return tile;
        }

        IEnumerator IEnumerable.GetEnumerator() => tileList.GetEnumerator();

        public FixedLengthTileModelQueue DeepCopy()
        {
            FixedLengthTileModelQueue copy=new FixedLengthTileModelQueue(Count);
            copy.tileList=DeepCopyObj<TileModel>.Array(tileList);
            copy.currentIndex=currentIndex;
            return copy;
        }
    }
}