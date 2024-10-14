namespace Knight.Domain.Models
{
    public class Weapon
    {
        public required string Name { get; set; }

        public required int Mod { get; set; }

        public required string Attr { get; set; }

        public required bool Equipped { get; set; }
    }
}