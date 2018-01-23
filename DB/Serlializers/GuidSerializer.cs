using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DB.Serlializers
{
    [BsonSerializer(typeof(GuidSerializer))]
    public class GuidSerializer : IBsonSerializer
    {
        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Guid.Parse(BsonSerializer.Deserialize<string>(context.Reader));
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            BsonSerializer.Serialize(context.Writer, value.ToString());
        }

        public Type ValueType
        {
            get { return typeof(Guid); }
        }
    }
}
