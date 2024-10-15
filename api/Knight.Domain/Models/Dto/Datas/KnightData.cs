using System.ComponentModel.DataAnnotations;

namespace Knight.Domain.Models.Dto.Datas
{
    public class KnightData
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        public ICollection<WeaponData> Weapons { get; set; }

        [Required]
        public AttributeData Attributes { get; set; }

        [Required]
        public string KeyAttribute { get; set; }
    }
}