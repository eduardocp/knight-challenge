using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace Knight.Domain.Models.Entities
{
    [Collection("knights")]
    public class Knight
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("nickName")]
        public string NickName { get; set; }

        [BsonElement("birthday")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Birthday { get; set; }

        [BsonElement("weapons")]
        public ICollection<Weapon> Weapons { get; set; }

        [BsonElement("attributes")]
        public Attributes Attributes { get; set; }

        [BsonElement("keyAttribute")]
        public string KeyAttribute { get; set; }
    }
}