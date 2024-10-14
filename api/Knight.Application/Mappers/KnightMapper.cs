using Riok.Mapperly.Abstractions;
using Knight.Domain.Models.Dto;
using Knight.Application.Helpers;

namespace Knight.Application.Mappers
{
    [Mapper]
    public partial class KnightMapper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Mapper", "RMG012:Source member was not found for target member")]
        private partial KnightDto MapToDto(Domain.Models.Knight knight);

        [UserMapping(Default = true)]
        public KnightDto? ToDto(Domain.Models.Knight? knight)
        {
            if (knight == null) return null;

            var dto = MapToDto(knight);
            
            dto!.Attack = KnightHelper.GetKnightAttack(knight);
            dto!.Age = (int)Math.Floor((DateTime.UtcNow - knight.Birthday).Days / 365m);

            var ageForExperience = dto.Age - 7;

            dto!.Experience = (int)Math.Floor(ageForExperience * Math.Pow(ageForExperience, 1.45));
            
            return dto;
        }

        public List<KnightDto> ListToDto(List<Domain.Models.Knight> knights)
        {
            var result = new List<KnightDto>();

            foreach (var knight in knights)
            {
                if (knight == null) continue;

                result.Add(ToDto(knight!)!);
            }

            return result;
        }
    }
}
