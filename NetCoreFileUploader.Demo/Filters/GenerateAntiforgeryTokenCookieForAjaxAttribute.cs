using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreFileUploader.Demo.Filters
{
    public class GenerateAntiforgeryTokenCookieForAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();

            var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);
            context.HttpContext.Response.Cookies.Append(
                "XSRF-TOKEN",
                tokens.RequestToken,
                new Microsoft.AspNetCore.Http.CookieOptions() { HttpOnly = false }
                );
        }
    }
}
