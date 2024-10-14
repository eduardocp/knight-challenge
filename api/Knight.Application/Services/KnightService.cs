using Knight.Application.Mappers;
using Knight.Domain.Interfaces;
using Knight.Domain.Models.Dto;
using Knight.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Knight.Application.Services
{
    public class KnightService(AppDbContext context) : IKnightService
    {
        private readonly AppDbContext _context = context;

        public async Task<List<KnightDto>> GetKnights(bool? onlyHeroes)
        {
            var mapper = new KnightMapper();

            if (onlyHeroes == true) return mapper.ListToDto(await _context.Knights.Where(k => (DateTime.UtcNow - k.Birthday).Days / 365 > 17).ToListAsync());

            return mapper.ListToDto(await _context.Knights.ToListAsync());
        }

        public async Task<KnightDto?> GetKnight(int id)
        {
            var mapper = new KnightMapper();

            return mapper.ToDto(await _context.Knights.FindAsync(id));
        }

        public async Task AddKnight(Domain.Models.Knight knight)
        {
            knight.Birthday = DateTime.SpecifyKind(knight.Birthday, DateTimeKind.Utc);

            _context.Knights.Add(knight);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateKnight(int id, Domain.Models.Knight knight)
        {
            var knightToUpdate = await _context.Knights.FindAsync(id);

            if (knightToUpdate != null)
            {
                knightToUpdate.Name = knight.Name;
                knightToUpdate.Birthday = DateTime.SpecifyKind(knight.Birthday, DateTimeKind.Utc);
                knightToUpdate.Weapons = knight.Weapons;
                knightToUpdate.Attributes = knight.Attributes;
                knightToUpdate.KeyAttribute = knight.KeyAttribute;
                knightToUpdate.NickName = knight.NickName;

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveKnight(int id)
        {
            var knightToUpdate = await _context.Knights.FindAsync(id);

            if (knightToUpdate != null)
            {
                _context.Knights.Remove(knightToUpdate);

                await _context.SaveChangesAsync();
            }
        }
    }
}