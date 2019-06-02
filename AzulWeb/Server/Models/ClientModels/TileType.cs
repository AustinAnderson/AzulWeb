using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Client
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TileType
    {
        Blue,
        Yellow,
        Red,
        Black,
        White,
        FirstPlayerMarker
    }
}
