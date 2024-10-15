using Knight.Domain.Models.Dto.Results;

namespace Knight.Domain.Interfaces
{
    public interface IKnightService
    {
        Task<List<KnightResult>> GetKnights(bool? onlyHeroes);

        Task<KnightResult?> GetKnight(string id);

        Task<KnightResult> AddKnight(Models.Dto.Datas.KnightData knight);

        Task<KnightResult> UpdateKnight(string id, Models.Dto.Datas.KnightData knight);

        Task RemoveKnight(string id);
    }
}