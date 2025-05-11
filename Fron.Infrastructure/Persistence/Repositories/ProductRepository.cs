using Fron.Application.Abstractions.Persistence;
using Fron.Domain.Dto.Product;
using Fron.Domain.Dto.ProductInventory;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class ProductRepository : BaseRepository, IProductRepository
{
    public ProductRepository(DataDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<GetProductResponseDto?> GetByIdAsync(int productId)
    {
        return await _context.Product
            .Where(x => x.ProductId == productId)
            .Select(x => new GetProductResponseDto(
                x.ProductId,
                x.Name,
                x.ProductNumber,
                x.MakeFlag,
                x.FinishedGoodsFlag,
                x.Color,
                x.SafetyStockLevel,
                x.ReorderPoint,
                x.StandardCost,
                x.ListPrice,
                x.ModifiedDate,
                x.ProductInventory != null && x.ProductInventory.Count > 0 ? x.ProductInventory.Select(y => new GetProductInventoryResponseDto(
                    y.ProductId,
                    y.LocationId,
                    y.Shelf,
                    y.Bin,
                    y.Quantity,
                    y.Rowguid,
                    y.ModifiedDate
                    )) : null))
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}
