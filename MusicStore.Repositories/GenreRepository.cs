using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;
namespace MusicStore.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly MusicStoreDbContext _context;

    public GenreRepository(MusicStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<Genre>> ListAsync()
    {
        return await _context.Set<Genre>()
            .Where(p => p.Status)
            .ToListAsync();

        //using (var cn = new SqlConnection(_context.Database.GetConnectionString()))
        //{
        //    using (var cm = cn.CreateCommand())
        //    {
        //        cm.CommandText = "SELECT * FROM Genre ORDER BY Name";
        //        await cn.OpenAsync();

        //        using (var dr = await cm.ExecuteReaderAsync())
        //        {
        //            var list = new List<Genre>();

        //            while (await dr.ReadAsync())
        //            {
        //                var entity = new Genre
        //                {
        //                    Id = dr.GetInt32(0),
        //                    Name = dr.GetString(1),
        //                    Status = dr.GetBoolean(2)
        //                };

        //                list.Add(entity);
        //            }

        //            return list;
        //        }
        //    }
        //}
    }

    public async Task<int> AddAsync(Genre entity)
    {
        await _context.Set<Genre>().AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<Genre?> FindByIdAsync(int id)
    {
        return await _context.Set<Genre>()
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Genre entity)
    {
        _context.Set<Genre>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Set<Genre>()
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        
        if (entity != null)
        {
            entity.Status = false;
            await _context.SaveChangesAsync();
        }
    }

}