using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Product;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GenericResponse<GetProductResponseDto>> GetProductByIdAsync(int Id)
    {
        var entity = await _productRepository.GetByIdAsync(Id);

        if (entity == null)
            return GenericResponse<GetProductResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        else
            return GenericResponse<GetProductResponseDto>.Success(entity, ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY, ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
    }
}
