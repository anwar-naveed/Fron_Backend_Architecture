using Fron.Application.Abstractions.Application;
using Microsoft.AspNetCore.Mvc;

namespace Fron.AdminApi.Controllers;
[Route("api/product")]
public class ProductController : BaseApiController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("Get-Product-Pdf")] //If HttpGet is used then getting CORS error in swagger but api is giving correct response in POSTMAN
    public async Task<IActionResult> GetProductPdfAsync(int Id)
    {
        var returnFileResponse = await _productService.GetProductPdfByIdAsync(Id);

        return File(returnFileResponse.Payload!.Stream, returnFileResponse.Payload.mimeType, returnFileResponse.Payload.FileName);
    }
}
