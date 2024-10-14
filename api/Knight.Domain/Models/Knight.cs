namespace Knight.Domain.Models
{
    public class Knight
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string NickName { get; set; }

        public required DateTime Birthday { get; set; }

        public required ICollection<Weapon> Weapons { get; set; }

        public Attributes Attributes { get; set; }

        public required string KeyAttribute { get; set; }
    }
}