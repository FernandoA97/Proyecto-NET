using MusicStore.Entities;
using MusicStore.Entities.Infos;

namespace MusicStore.Repositories;

public interface IConcertRepository
{
    Task<ICollection<ConcertInfo>> ListAsync(string? filter, int page, int rows);
    Task<Concert?> FindByIdAsync(int id);
    Task<int> AddAsync(Concert entity);
}