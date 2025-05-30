using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fron.ApiProjectExtensions.Middlewares;

public class HeadersHandlingMiddleware
{
    private readonly RequestDelegate _next;
    //private readonly ILogger<HeadersHandlingMiddleware> _logger;

    public HeadersHandlingMiddleware(RequestDelegate next
        //, ILogger<HeadersHandlingMiddleware> logger
        )
    {
        _next = next;
        //_logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var parent = Directory.GetParent(Directory.GetCurrentDirectory());
        var file = Path.Combine(parent!.FullName, "Test.txt");
        //File.Create(file);
        await File.AppendAllTextAsync(file, $"Path: {context.Request.Path}");

        foreach (var header in context.Request.Headers)
        {
            //_logger.LogInformation("Header: {Key}: {Value}", header.Key, header.Value);
            await File.AppendAllTextAsync(file, $"Header: {header.Key}: {header.Value}");
        }


        await _next(context);
    }
}