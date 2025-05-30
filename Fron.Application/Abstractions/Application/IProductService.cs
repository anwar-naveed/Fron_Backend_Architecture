﻿using Fron.Domain.Dto.Product;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IProductService
{
    Task<GenericResponse<GetProductResponseDto>> GetProductByIdAsync(int Id);
}