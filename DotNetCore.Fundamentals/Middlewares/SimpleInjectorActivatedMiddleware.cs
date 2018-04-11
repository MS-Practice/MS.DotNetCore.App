using CommonCodeProject.Data;
using CommonCodeProject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Fundamentals.Middlewares
{
    public class SimpleInjectorActivatedMiddleware : IMiddleware
    {
        private readonly AppDbContext _db;
        public SimpleInjectorActivatedMiddleware(AppDbContext db)
        {
            _db = db;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var keyValue = context.Request.Query["key"];

            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                _db.Add(new Request()
                {
                    DT = DateTime.UtcNow,
                    MiddlewareActivation = "SimpleInjectorActivatedMiddleware",
                    Value = keyValue
                });

                await _db.SaveChangesAsync();
            }

            await next(context);
        }
    }
}
