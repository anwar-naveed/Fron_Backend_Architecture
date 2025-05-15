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

    [HttpGet("Get-Product-By-Id")] //Use HttpPost if getting CORS error in swagger but api is giving correct response in POSTMAN
    public async Task<IActionResult> GetProductByIdAsync(int Id)
        =>Ok(await _productService.GetProductByIdAsync(Id));
}
