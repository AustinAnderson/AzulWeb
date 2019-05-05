using System;

namespace Server.Models.Cloning
{
    public interface IDeepCopyable<T>
    {
        T DeepCopy();
    }
}

