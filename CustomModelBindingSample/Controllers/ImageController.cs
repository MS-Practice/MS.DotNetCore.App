using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomModelBindingSample.Controllers
{
    [Produces("application/json")]
    [Route("api/image")]
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImageController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        #region post snippet1
        [HttpPost]
        public void Post(byte[] file, string filename)
        {
            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/images/upload", filename);
            if (System.IO.File.Exists(filePath)) return;
            System.IO.File.WriteAllBytes(filePath, file);
        } 
        #endregion
        [HttpPost("Profile")]
        public void SaveProfile(ProfileViewModel model)
        {
            string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot/images/upload", model.FileName);
            System.IO.File.WriteAllBytes(filePath, model.File);
        }

        public class ProfileViewModel
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
        }
    }
}
