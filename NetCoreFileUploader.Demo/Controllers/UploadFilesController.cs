using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace NetCoreFileUploader.Demo.Controllers
{
    public class UploadFilesController : Controller
    {
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if(formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath,FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { content = files.Count, size, filePath });
        }
    }
}