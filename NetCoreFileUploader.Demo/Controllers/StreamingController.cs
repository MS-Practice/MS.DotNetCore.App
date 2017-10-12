using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text;
using NetCoreFileUploader.Demo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace NetCoreFileUploader.Demo.Controllers
{
    public class StreamingController : Controller
    {
        private readonly ILogger<StreamingController> _logger;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public StreamingController(ILogger<StreamingController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        // 1. Disable the form value model binding here to take control of handling 
        //    potentially large files.
        // 2. Typically antiforgery tokens are sent in request body, but since we 
        //    do not want to read the request body early, the tokens are made to be 
        //    sent via headers. The antiforgery token filter first looks for tokens
        //    in the request header and then falls back to reading the body.
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType),_defaultFormOptions.MultipartBoundaryLengthLimit);

            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section!=null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        targetFilePath = Path.GetTempFileName();
                        using (var targetStream = System.IO.File.Create(targetFilePath))
                        {
                            await section.Body.CopyToAsync(targetStream);
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"
                        //
                        // value

                        // Do not limit the key name length here because the 
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);

                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks:true,
                            bufferSize:1024,
                            leaveOpen:true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = string.Empty;
                            }
                            formAccumulator.Append(key.Value, value);
                            if(formAccumulator.ValueCount > _defaultFormOptions.ValueLengthLimit)
                            {
                                throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
                            }
                        }
                    }
                }
                //Drains any remaining section body has not been consumed and
                //read the headers for the next section
                section = await reader.ReadNextSectionAsync();
            }

            //Bind form data to a model
            var user = new User();
            var formValueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(formAccumulator.GetResults()),
                CultureInfo.CurrentCulture);

            var bindingSuccessFul = await TryUpdateModelAsync(user, prefix: "", valueProvider: formValueProvider);
            if (!bindingSuccessFul)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            var uploadedData = new UploadedData()
            {
                Name = user.Name,
                Age = user.Age,
                ZipCode = user.ZipCode,
                FilePath = targetFilePath
            };
            return Json(uploadedData);
        }

        private Encoding GetEncoding(MultipartSection section)
        {
            throw new NotImplementedException();
        }
    }
}