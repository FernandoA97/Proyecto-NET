using Microsoft.EntityFrameworkCore;
using MusicStore.DataAccess;
using MusicStore.Entities;

namespace MusicStore.Repositories;

public class SaleRepository : RepositoryBase<Sale>, ISaleRepository
{
    public SaleRepository(MusicStoreDbContext context) : base(context)
    {
        
    }

    public async Task<int> CreateSaleAsync(Sale entity)
    {
        entity.SaleDate = DateTime.Now;
        var lastNumber = await Context.Set<Sale>().CountAsync() + 1;
        entity.OperationNumber = $"{lastNumber:000000}"; // 000001

        await Context.Set<Sale>().AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity.Id;
    }
}