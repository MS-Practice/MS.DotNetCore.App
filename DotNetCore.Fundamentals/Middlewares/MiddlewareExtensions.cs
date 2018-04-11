using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Fundamentals.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSimpleInjectorActivatedMiddleware(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleInjectorActivatedMiddleware>();
        }
    }
}
