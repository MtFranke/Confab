using System;

namespace Confab.Shared.Infrastructure.Modules
{
    public interface IModuleSerializer
    {
        byte[] Serializer<T>(T value);
        T Deserializer<T>(byte[] value);
        object Deserializer(byte[] value, Type type);
    }
}