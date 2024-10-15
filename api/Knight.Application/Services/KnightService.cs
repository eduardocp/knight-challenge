using Knight.Application.Mappers;
using Knight.Domain.Interfaces;
using Knight.Domain.Models.Dto.Results;
using Knight.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Knight.Application.Services
{
    public class KnightService(AppDbContext context) : IKnightService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<KnightResult>> GetKnights(bool? onlyHeroes)
        {
            var mapper = new KnightMapper();

            if (onlyHeroes == true) return mapper.ListToResult(await _context.Knights.ToListAsync()).Where(k => (DateTime.UtcNow - k.Birthday).Days / 365 > 7).ToList();

            return mapper.ListToResult(await _context.Knights.ToListAsync());
        }

        public async Task<KnightResult?> GetKnight(string id)
        {
            var mapper = new KnightMapper();

            return mapper.ToResult(await _context.Knights.FindAsync(ObjectId.Parse(id)));
        }

        public async Task<KnightResult> AddKnight(Domain.Models.Dto.Datas.KnightData knight)
        {
            var existsAKnightSameName = await _context.Knights.Where(x => x.Name == knight.Name).AnyAsync();

            if (existsAKnightSameName) throw new Exception("A knight with the same name already exists.");

            knight.Birthday = DateTime.SpecifyKind(knight.Birthday, DateTimeKind.Utc);

            var mapper = new KnightMapper();
            var mappedKnight = mapper.MapToEntity(knight);

            _context.Knights.Add(mappedKnight);

            await _context.SaveChangesAsync();

            return mapper.ToResult(mappedKnight)!;
        }

        public async Task<KnightResult> UpdateKnight(string id, Domain.Models.Dto.Datas.KnightData knight)
        {
            var existsAKnightSameName = await _context.Knights.Where(x => x.Name == knight.Name && x.Id != ObjectId.Parse(id)).AnyAsync();

            if (existsAKnightSameName) throw new Exception("A knight with the same name already exists.");

            var knightToUpdate = await _context.Knights.FindAsync(ObjectId.Parse(id));

            var mapper = new KnightMapper();
            var mappedKnight = mapper.MapToEntity(knight);

            knightToUpdate!.Name = mappedKnight.Name;
            knightToUpdate.Birthday = DateTime.SpecifyKind(mappedKnight.Birthday, DateTimeKind.Utc);
            knightToUpdate.Weapons = mappedKnight.Weapons;
            knightToUpdate.Attributes = mappedKnight.Attributes;
            knightToUpdate.KeyAttribute = mappedKnight.KeyAttribute;
            knightToUpdate.NickName = mappedKnight.NickName;

            await _context.SaveChangesAsync();

            return mapper.ToResult(mappedKnight)!;
        }

        public async Task RemoveKnight(string id)
        {
            var knightToUpdate = await _context.Knights.FindAsync(ObjectId.Parse(id));

            if (knightToUpdate != null)
            {
                _context.Knights.Remove(knightToUpdate);

                await _context.SaveChangesAsync();
            }
        }
    }
}