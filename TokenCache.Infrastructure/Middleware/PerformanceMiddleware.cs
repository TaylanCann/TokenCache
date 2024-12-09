using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Infrastructure.Middleware
{
    public class PerformanceMiddleware
    {
        private readonly RequestDelegate _next;

        // Constructor: Bu middleware'in bir sonraki middleware ile bağlantısını sağlar
        public PerformanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // InvokeAsync metodu: Her istek geldiğinde çalışacak ana metoddur
        public async Task InvokeAsync(HttpContext context)
        {
            // İstek zamanını kaydediyoruz
            var start = DateTime.UtcNow;

            // Bir sonraki middleware'e geçiyoruz
            await _next(context);

            // İşlem süresini hesaplıyoruz
            var duration = DateTime.UtcNow - start;
            Console.WriteLine($"Request took {duration.TotalMilliseconds} ms");
        }
    }
}
