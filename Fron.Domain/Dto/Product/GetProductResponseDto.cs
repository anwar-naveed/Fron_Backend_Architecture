using Fron.Domain.Dto.ProductInventory;

namespace Fron.Domain.Dto.Product;
public sealed record GetProductResponseDto(
    int ProductId,
    string Name,
    string ProductNumber,
    bool MakeFlag,
    bool FinishedGoodsFlag,
    string? Color,
    short SafetyStockLevel,
    short ReorderPoint,
    decimal StandardCost,
    decimal ListPrice,
    DateTime ModifiedDate,
    IEnumerable<GetProductInventoryResponseDto>? ProductInventories
);
