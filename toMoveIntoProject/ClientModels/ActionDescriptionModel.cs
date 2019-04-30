using System;
namespace Models.Client
{
    public class ActionDescriptionModel 
    {
        public bool FromFactory {get;set;}
        public TileType tileType {get;set;}
        public int FactoryIndex {get;set;}
        public int PatternLineIndex {get;set;}
    }
}
