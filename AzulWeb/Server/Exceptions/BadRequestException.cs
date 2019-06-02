using System;
namespace Server.Exceptions
{
    [System.Serializable]
    public class BadRequestException : System.Exception
    {
        public bool BootableOffence{get;set;}
        public BadRequestException(bool bootable) =>BootableOffence=bootable;
        public BadRequestException(string message,bool bootable) : base(message)=>BootableOffence=bootable;
        public BadRequestException(string message,bool bootable, System.Exception inner) : base(message, inner) 
        { 
            BootableOffence=bootable;
        }
        protected BadRequestException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

