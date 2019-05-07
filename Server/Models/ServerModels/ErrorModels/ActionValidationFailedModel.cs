using System;
namespace Server.Models.ServerModels.ErrorModels
{
    public class ActionValidationFailedModel
    {
        public bool ErrorOnFactory {get;set;}
        public int FactoryIndex {get;set;}
        public string Message{get;set;}
    }
}

