using System;
namespace Server.Models.ServerModels.ErrorModels
{
    public class ActionValidationFailedModel
    {
        public bool ErrorOnCenterOfTable {get;set;}
        public int? FactoryIndex {get;set;}
        public int? PatternLineIndex {get;set;}
        public string Message{get;set;}
    }
}

