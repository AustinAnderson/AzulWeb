using System;
namespace Server
{
    public static class ClientFacingMessages
    {
        public static string NoDrawingFromEmptyFactory()
            =>"Can't draw tiles from an empty factory";
        public static string InvalidTileTypeForFactory(string tileType)
            =>$"Can't draw {tileType} tiles from Factory because it doesn't have any of that color";
        public static string NoDrawingFromEmptyCenterOfTable()
            =>"Can't draw tiles from center of table since it has no tiles";
        public static string InvalidTileTypeForCenterOfTable(string tileType)
            =>$"Can't draw {tileType} tiles from center of table because it doesn't have any of that color";
        public static string PatternLineAlreadyFull(string rowNumber)
            =>$"Can't move tiles to the {rowNumber} row because it's already full";
        public static string PatternLineContainsDifferentType(
            string chosenTileType,string existingTileType,string rowNumber
        ){
            return $"Can't move {chosenTileType} tiles to the {rowNumber} row "+
            $"because it's already contains {existingTileType} tiles";
        }
    }
}

