using Microsoft.AspNetCore.Mvc;
using NetCoreFileUploader.Demo.Models;
using Microsoft.Extensions.Options;

namespace NetCoreFileUploader.Demo.Controllers
{
    public class SettingsController : Controller
    {
        private readonly SampleWebSettings _webSettings;

        public SettingsController(IOptions<SampleWebSettings> settingsOptions)
        {
            _webSettings = settingsOptions.Value;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = _webSettings.Title;
            ViewData["Updates"] = _webSettings.Updates;
            return View();
        }
    }
}