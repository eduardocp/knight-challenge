using MongoDB.Bson.Serialization.Attributes;

namespace Knight.Domain.Models.Entities
{
    public class Attributes
    {
        [BsonElement("strength")]
        public int Strength { get; set; }

        [BsonElement("dexterity")]
        public int Dexterity { get; set; }

        [BsonElement("constitution")]
        public int Constitution { get; set; }

        [BsonElement("intelligence")]
        public int Intelligence { get; set; }

        [BsonElement("wisdom")]
        public int Wisdom { get; set; }

        [BsonElement("charisma")]
        public int Charisma { get; set; }
    }
}