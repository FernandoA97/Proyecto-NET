using MusicStore.Entities;
namespace MusicStore.DataAccess.Repositories;

public class GenreRepository
{
    private MusicStoreDbContext _context;

    public GenreRepository(MusicStoreDbContext context)
    {
        _context = context;
    }

    public List<Genre> List(){
        return _context.Set<Genre>().ToList();
    }

    public void Add(Genre entity){
        _context.Set<Genre>().Add(entity);
        _context.SaveChanges();
    }

}