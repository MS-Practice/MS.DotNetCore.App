using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Fundamentals.Localization
{
    public static class LocalizationExtensions
    {
        public static void AddLocalization(this IServiceCollection services,string resourcePath = "Resources")
        {
            // 添加本地化容器
            services.AddLocalization(options =>
            {
                options.ResourcesPath = resourcePath;
            });

            services.AddMvc()
                .AddMvcLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
        }
    }
}
