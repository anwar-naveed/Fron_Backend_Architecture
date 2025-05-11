using Fron.Domain.Dto.Product;

namespace Fron.Application.Abstractions.Persistence;
public interface IProductRepository
{
    Task<GetProductResponseDto?> GetByIdAsync(int productId);
}