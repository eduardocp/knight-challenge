using Knight.Application.Helpers;
using Knight.Domain.Models.Dto.Results;
using MongoDB.Bson;
using Riok.Mapperly.Abstractions;

namespace Knight.Application.Mappers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
    public partial class KnightMapper
    {
        private partial KnightResult MapToResult(Domain.Models.Entities.Knight knight);

        public partial Domain.Models.Entities.Knight MapToEntity(Domain.Models.Dto.Datas.KnightData knight);

        [UserMapping(Default = true)]
        public KnightResult? ToResult(Domain.Models.Entities.Knight? knight)
        {
            if (knight == null) return null;

            var dto = MapToResult(knight);

            dto!.Age = (int)Math.Floor((DateTime.UtcNow - knight.Birthday).Days / 365m);
            dto!.Attack = KnightHelper.GetKnightAttack(knight);

            var ageForExperience = dto.Age - 7;

            if (ageForExperience > 0)
            {
                dto!.Experience = (int)Math.Floor(ageForExperience * Math.Pow(ageForExperience, 1.45));
            }

            return dto;
        }

        public List<KnightResult> ListToResult(List<Domain.Models.Entities.Knight> knights)
        {
            var result = new List<KnightResult>();

            foreach (var knight in knights)
            {
                if (knight == null) continue;

                result.Add(ToResult(knight!)!);
            }

            return result;
        }

        private string ObjectIdToString(ObjectId id) => id.ToString();
    }
}