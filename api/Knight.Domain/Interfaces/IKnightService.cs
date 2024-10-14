namespace Knight.Domain.Interfaces
{
    public interface IKnightService
    {
        Task<List<Models.Dto.KnightDto>> GetKnights(bool? onlyHeroes);

        Task<Models.Dto.KnightDto?> GetKnight(int id);

        Task AddKnight(Models.Knight knight);

        Task UpdateKnight(int id, Models.Knight knight);

        Task RemoveKnight(int id);
    }
}