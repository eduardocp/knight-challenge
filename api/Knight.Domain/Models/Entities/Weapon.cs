using MongoDB.Bson.Serialization.Attributes;

namespace Knight.Domain.Models.Entities
{
    public class Weapon
    {
        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonElement("mod")]
        public required int Mod { get; set; }

        [BsonElement("attr")]
        public required string Attr { get; set; }

        [BsonElement("equipped")]
        public required bool Equipped { get; set; }
    }
}