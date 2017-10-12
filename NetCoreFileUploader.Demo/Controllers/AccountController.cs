using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreFileUploader.Demo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using NetCoreFileUploader.Demo.Models;
using System.IO;

namespace NetCoreFileUploader.Demo.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewData["ReturnUrl"] = "";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                using (var momeryStream = new MemoryStream())
                {
                    await model.AvatarImage.CopyToAsync(momeryStream);
                    user.AvatarImage = momeryStream.ToArray();
                }
            }
            return Ok();
        }
    }
}