using System.ComponentModel.DataAnnotations;

namespace Knight.Domain.Models.Dto.Datas
{
    public class WeaponData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Mod { get; set; }

        [Required]
        public string Attr { get; set; }

        [Required]
        public bool Equipped { get; set; }
    }
}