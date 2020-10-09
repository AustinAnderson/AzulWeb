using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Models.ServerModels.ErrorModels
{
    public enum HttpMethodEnum
    {
        GET,
        PUT,
        POST,
        PATCH,
        DELETE
    }
    public class BadRequestModel
    {
        public BadRequestModel(string message) : this(message, null, null) { }
        public BadRequestModel(string message, string seeUrl,HttpMethodEnum? httpMethod)
        {
            Message = message;
            SeeUrl = seeUrl;
            Method = httpMethod;
        }

        public string Message { get; set; }
        public string SeeUrl { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public HttpMethodEnum? Method { get; set; }
    }
}
