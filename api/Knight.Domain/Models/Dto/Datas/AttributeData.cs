using System.ComponentModel.DataAnnotations;

namespace Knight.Domain.Models.Dto.Datas
{
    public class AttributeData
    {
        [Required]
        public int Strength { get; set; }

        [Required]
        public int Dexterity { get; set; }

        [Required]
        public int Constitution { get; set; }

        [Required]
        public int Intelligence { get; set; }

        [Required]
        public int Wisdom { get; set; }

        [Required]
        public int Charisma { get; set; }
    }
}