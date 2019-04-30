using System;
using System.Collections.Generic;
namespace Models.Client
{
    public class SharedDataModel
    {
        public ConfigModel Config {get;set;}
        public List<TileModel> DiscardPile {get;set;}
        public List<TileModel> CenterOfTable {get;set;}
        public List<TileModel> Bag {get;set;}
        public TileModel[][] Factories {get;set;}
        public override int GetHashCode()
        {
            int hash=17;
            unchecked
            {
                if(Config!=null)
                {
                    hash=hash*31+Config.GetHashCode();
                }
                if(DiscardPile!=null)
                {
                    for(int i=0;i<DiscardPile.length;i++)
                    {
                        if(DiscardPile[i]!=null)
                            hash=hash*31+DiscardPile[i].GetHashCode();
                    }
                }
                if(CenterOfTable!=null)
                {
                    for(int i=0;i<CenterOfTable.length;i++)
                    {
                        if(CenterOfTable[i]!=null)
                            hash=hash*31+CenterOfTable[i].GetHashCode();
                    }
                }
                if(Bag!=null)
                {
                    for(int i=0;i<Bag.length;i++)
                    {
                        if(Bag[i]!=null)
                            hash=hash*31+Bag[i].GetHashCode();
                    }
                }
                if(Factories!=null)
                {
                    for(int i=0;i<Factories.length;i++){
                        for(int j=0;j<Factories[i]?.length??0;j++){
                            if(Factories[i][j]!=null)
                                hash=hash*31+Factories[i][j].GetHashCode();
                        }
                    }
                }
            }
            return hash;
        }
    }
}
