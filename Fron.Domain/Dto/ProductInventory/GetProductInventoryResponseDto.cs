namespace Fron.Domain.Dto.ProductInventory;
public sealed record GetProductInventoryResponseDto(
    int ProductId,
    short LocationId,
    string Shelf,
    byte Bin,
    short Quantity,
    Guid Rowguid,
    DateTime ModifiedDate
);
