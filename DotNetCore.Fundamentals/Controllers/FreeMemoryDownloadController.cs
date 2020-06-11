using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using DotNetCore.Fundamentals.ActionResults;
using Ionic.Zip;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Fundamentals.Controllers {
    [ApiController]
    [Route("api/freememory")]
    public class FreeMemoryDownloadController : ControllerBase {
        private static HttpClient Client { get; } = new HttpClient();

        public async Task<IActionResult> Get() {
            var stream = await Client.GetStreamAsync("https://raw.githubusercontent.com/MS-Practice/MS.DotNetCore.App/master/README.md");

            return new FileStreamResult(stream, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("text/plain")) {
                FileDownloadName = "README.md"
            };
        }
        /// <summary>
        /// 压缩时，引入了 Memory，在压缩的时候，会把多个文件的流加载到 Memory
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetZipNotBetter() {
            var filenamesAndUrls = new Dictionary<string, string> {
                    ["README.md"] = "https://raw.githubusercontent.com/MS-Practice/MS.DotNetCore.App/master/README.md",
                    [".gitignore"] = "https://raw.githubusercontent.com/MS-Practice/MS.DotNetCore.App/master/.gitignore"
                };

            var archive = new MemoryStream();
            using var zipStream = new ZipOutputStream(archive, leaveOpen : true);
            foreach (var kvp in filenamesAndUrls) {
                zipStream.PutNextEntry(kvp.Key);
                using var stream = await Client.GetStreamAsync(kvp.Value);
                await stream.CopyToAsync(zipStream);
            }

            archive.Position = 0;
            var result = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StreamContent(archive)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(DispositionTypeNames.Attachment) {
                FileName = "MyZipfile.zip"
            };
            return result;
        }

        public async Task<IActionResult> GetZipBetter() {
            var filenamesAndUrls = new Dictionary<string, string> {
                    ["README.md"] = "https://raw.githubusercontent.com/MS-Practice/MS.DotNetCore.App/master/README.md",
                    [".gitignore"] = "https://raw.githubusercontent.com/MS-Practice/MS.DotNetCore.App/master/.gitignore"
                };
            await Task.Delay(1);
            return new FileCallbackResult(new System.Net.Http.Headers.MediaTypeHeaderValue(MediaTypeNames.Application.Octet), async(outputStream, _) => {
                using var zipArchive = new ZipArchive(new WriteOnlyStreamWrapper(outputStream), ZipArchiveMode.Create);
                foreach (var kvp in filenamesAndUrls) {
                    var zipEntry = zipArchive.CreateEntry(kvp.Key);
                    using var zipStream = zipEntry.Open();
                    using var stream = await Client.GetStreamAsync(kvp.Value);
                    await stream.CopyToAsync(zipStream);
                }
            }) {
                FileDownloadName = "MyZipfile.zip"
            };
        }
    }
}