using System;
using Models.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server.Serialization
{
    public class FixedLengthTileModelQueueConverter : JsonConverter<FixedLengthTileModelQueue>
    {
        public override FixedLengthTileModelQueue ReadJson(
            JsonReader reader, Type objectType, FixedLengthTileModelQueue existingValue, 
            bool hasExistingValue, JsonSerializer serializer
        )
        {
            //if(hasExistingValue) 
                //return existingValue;
            if(objectType != typeof(FixedLengthTileModelQueue)) 
                throw new InvalidCastException($"cannot deserialize {objectType.Name} to {nameof(FixedLengthTileModelQueue)}");
            JArray array=JArray.Load(reader);
            var list=array.ToObject<TileModel[]>();
            FixedLengthTileModelQueue toReturn=new FixedLengthTileModelQueue(list.Length);
            foreach(var model in list){
                toReturn.TryAdd(model);
            }
            return toReturn;
        }

        public override void WriteJson(JsonWriter writer, FixedLengthTileModelQueue value, JsonSerializer serializer)
        {
            var toSerialize=new TileModel[value.Count];
            int i=0;
            foreach(var model in value){
                toSerialize[i]=model;
                i++;
            }
            serializer.Serialize(writer,toSerialize);
        }
    }
}

