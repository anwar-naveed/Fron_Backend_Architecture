using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Infrastructure;
using Fron.Application.Abstractions.Persistence;
using Fron.Domain.Configuration;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Product;
using Fron.Domain.GenericResponse;
using Microsoft.Extensions.Options;

namespace Fron.Application.Services;
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IDocumentService _documentService;
    private readonly TemplateConfiguraion _templateConfiguration;

    public ProductService(
        IProductRepository productRepository,
        IDocumentService documentService,
        IOptions<TemplateConfiguraion> templateConfiguration)
    {
        _productRepository = productRepository;
        _documentService = documentService;
        _templateConfiguration = templateConfiguration.Value;
    }

    public async Task<GenericResponse<ProductFileResponseDto>> GetProductPdfByIdAsync(int Id)
    {
        var entity = await _productRepository.GetByIdAsync(Id);
        var stream = new MemoryStream();
        stream.Position = 0;
        string pdfName = $"{FileNames.PRODUCT_PDF_FILE}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{FileExtensions.PDF}";
        string sourceTemplate = $@"{_templateConfiguration.TemplateDirectory}/Html/{FileNames.PRODUCT_INVENTORY_PDF_FILE}{FileExtensions.HTML}";
        if (!File.Exists(sourceTemplate))
        {
            ProductFileResponseDto responseDto = new ProductFileResponseDto(pdfName, stream, MimeTypes.PDF);
            return GenericResponse<ProductFileResponseDto>.Failure(responseDto, ApiResponseMessages.TEMPLATE_NOT_FOUND, ApiStatusCodes.TEMPLATE_NOT_FOUND);
        }
        else if (entity == null)
        {
            ProductFileResponseDto responseDto = new ProductFileResponseDto(pdfName, stream, MimeTypes.PDF);
            return GenericResponse<ProductFileResponseDto>.Failure(responseDto, ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            var templateContent = File.ReadAllText(sourceTemplate);

            // Generate PDF stream using the document service
            var pdfStream = _documentService.GeneratePDFStream(
                entity, 
                templateContent, 
                Path.GetFullPath($@"{_templateConfiguration.TemplateDirectory}/Html/"), 
                stream);
            pdfStream.Position = 0;

            ProductFileResponseDto responseDto = new ProductFileResponseDto(pdfName, pdfStream, MimeTypes.PDF);

            return GenericResponse<ProductFileResponseDto>.Success(
                responseDto,
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }

    }
}
