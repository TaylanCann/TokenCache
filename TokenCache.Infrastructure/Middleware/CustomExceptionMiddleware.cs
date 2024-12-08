using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TokenCache.Application.Exceptions;

namespace TokenCache.Infrastructure.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Bir sonraki middleware'e geç
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Varsayılan durum kodu
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new { error = exception.Message };

            if (exception is UserNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response = new { error = exception.Message };
            }

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }

    }
}
