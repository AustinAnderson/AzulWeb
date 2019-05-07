using System;
using System.Collections.Generic;
using Models.Hashing;
using Server.Models.Cloning;

namespace Models.Client
{
    public class SharedDataModel
    {
        public ConfigModel Config {get;set;}
        public List<TileModel> DiscardPile {get;set;}
        public List<TileModel> CenterOfTable {get;set;}
        public List<TileModel> Bag {get;set;}
        public FactoryModel[] Factories {get;set;}

        public override int GetHashCode()
        {
            int hash=17;
            ModelHashUtils.CombineHash(ref hash,Config?.GetHashCode());
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(DiscardPile));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(CenterOfTable));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(Bag));
            ModelHashUtils.CombineHash(ref hash,ModelHashUtils.HashList(Factories));
            return hash;
        }
    }
}
