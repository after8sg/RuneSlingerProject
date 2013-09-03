
using System;
namespace RuneSlinger.server.Abstract
{
    public interface ISerializer
    {
        byte[] Serialize(object @object);
        object Deserialize(byte[] bytes, Type type);
    }
}
