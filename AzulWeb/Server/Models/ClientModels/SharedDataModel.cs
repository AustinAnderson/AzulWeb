using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class SharedDataModel:IDeepCopyable<SharedDataModel>
    {
        public ConfigModel Config {get;set;}=new ConfigModel();
        public HashSet<TileModel> DiscardPile {get;set;}=new HashSet<TileModel>();
        public HashSet<TileModel> CenterOfTable {get;set;}=new HashSet<TileModel>();
        public HashSet<TileModel> Bag {get;set;}=new HashSet<TileModel>();
        public FactoryModel[] Factories {get;set;}
        public int CurrentTurnsPlayersIndex{get;set;}=0;


        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,Config?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfSet(DiscardPile));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfSet(CenterOfTable));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfSet(Bag));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashOfEnumerable(Factories));
            ModelHashUtils.CombineHash(ref hash,CurrentTurnsPlayersIndex);
            return hash;
        }

        public SharedDataModel DeepCopy()=> new SharedDataModel
        {
            CurrentTurnsPlayersIndex=this.CurrentTurnsPlayersIndex,
            Config=this.Config?.DeepCopy(),
            DiscardPile=DeepCopyObj<TileModel>.Set(this.DiscardPile),
            CenterOfTable=DeepCopyObj<TileModel>.Set(this.CenterOfTable),
            Bag=DeepCopyObj<TileModel>.Set(this.Bag),
            Factories=DeepCopyObj<FactoryModel>.Array(this.Factories)
        };
    }
}
