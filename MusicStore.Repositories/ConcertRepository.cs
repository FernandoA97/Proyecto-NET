using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;
using MusicStore.Entities.Infos;

namespace MusicStore.Repositories;

public class ConcertRepository : IConcertRepository
{
    private readonly MusicStoreDbContext _context;

    public ConcertRepository(MusicStoreDbContext context)
    {
        _context = context;
    }
    public async Task<ICollection<ConcertInfo>> ListAsync(string? filter, int page, int rows)
    {
        return await _context.Set<Concert>()
            .Where(p => p.Status
            && (p.Title.Contains(filter ?? string.Empty)))
            .OrderByDescending(p => p.DateEvent)
            .Skip((page - 1) * rows)
            .Take(rows)
            .Select(p => new ConcertInfo
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                DateEvent = p.DateEvent.ToString("yyyy-MM-dd"),
                TimeEvent = p.DateEvent.ToString("HH:mm:ss"),
                Genre = p.Genre.Name, // Lazy Loading
                UnitPrice = p.UnitPrice,
                ImageUrl = p.ImageUrl,
                TicketsQuantity = p.TicketsQuantity,
                Status = p.Finalized ? "Finalizado" : "Pendiente"
            })
            .ToListAsync();
    }

    public async Task<Concert?> FindByIdAsync(int id)
    {
        return await _context.Set<Concert>()
            .Include(p => p.Genre)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> AddAsync(Concert entity)
    {
        await _context.Set<Concert>().AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }
}