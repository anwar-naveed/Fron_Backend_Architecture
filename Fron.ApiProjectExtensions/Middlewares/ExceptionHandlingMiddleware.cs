using Fron.Application.Abstractions.Infrastructure;
using Fron.Domain.Constants;
using Fron.Domain.GenericResponse;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace Fron.ApiProjectExtensions.Middlewares;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context,
        ILoggingService loggingService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {

            if (_env.IsProduction())
            {
                await loggingService.LogExceptionAsync(ex);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var payload = ex.InnerException != null ? ex.InnerException.ToString() : string.Empty;

            var response = _env.IsDevelopment() || _env.IsProduction() ?
              GenericResponse<string>.Failure(payload, ex.Message, (int)HttpStatusCode.InternalServerError) :
              GenericResponse<string>.Failure(ApiResponseMessages.SOMETHING_WENT_WRONG, (int)HttpStatusCode.InternalServerError);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
