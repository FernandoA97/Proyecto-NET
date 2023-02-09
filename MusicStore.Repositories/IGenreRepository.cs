using MusicStore.Entities;

namespace MusicStore.Repositories;

public interface IGenreRepository
{
    Task<List<Genre>> ListAsync();
    Task<int> AddAsync(Genre entity);
    Task<Genre?> FindByIdAsync(int id);
    Task UpdateAsync(Genre entity);
    Task DeleteAsync(int id);
}