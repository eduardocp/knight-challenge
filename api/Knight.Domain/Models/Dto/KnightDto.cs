namespace Knight.Domain.Models.Dto
{
    public class KnightDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string NickName { get; set; }

        public required DateTime Birthday { get; set; }

        public int Age { get; set; }

        public required ICollection<Weapon> Weapons { get; set; }

        public Attributes Attributes { get; set; }

        public required string KeyAttribute { get; set; }

        public int Experience { get; set; }

        public int Attack { get; set; }
    }
}
