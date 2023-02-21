using MusicStore.Entities;

namespace MusicStore.Repositories
{
    public interface ISaleRepository : IRepositoryBase<Sale>
    {
        Task<int> CreateSaleAsync(Sale entity);
    }
}
