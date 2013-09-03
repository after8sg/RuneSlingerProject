
using Newtonsoft.Json;
using RuneSlinger.server.Abstract;
using System.IO;
using Newtonsoft.Json.Bson;
namespace RuneSlinger.server
{
    public class BSONSerializer : ISerializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public BSONSerializer()
        {
            _jsonSerializer = new JsonSerializer();
        }

        public byte[] Serialize(object @object)
        {
            using (var ms = new MemoryStream())
            {
                
                _jsonSerializer.Serialize(new BsonWriter(ms), @object);
                return ms.ToArray();
            }
        
        }

        public object Deserialize(byte[] bytes, System.Type type)
        {
            using (var ms = new MemoryStream(bytes))
                return _jsonSerializer.Deserialize(new BsonReader(ms), type);
        }
    }
}
