using System;
using System.Text;
using System.Text.Json;

namespace Confab.Shared.Infrastructure.Modules
{
    internal sealed class JsonModuleSerializer : IModuleSerializer
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public byte[] Serializer<T>(T value) =>
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, SerializerOptions));

        public T Deserializer<T>(byte[] value) =>
            JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(value), SerializerOptions);

        public object Deserializer(byte[] value, Type type) =>
            JsonSerializer.Deserialize(Encoding.UTF8.GetString(value), type, SerializerOptions);
    }
}