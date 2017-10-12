using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreFileUploader.Demo
{
    public class MultipartRequestHelper
    {
        public static string GetBoundary(MediaTypeHeaderValue contentType,int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            if (string.IsNullOrWhiteSpace(boundary.Value))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary.Value;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrWhiteSpace(contentType) && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                    && contentDisposition.DispositionType.Equals("form-data")
                    && string.IsNullOrWhiteSpace(contentDisposition.FileName.Value)
                    && string.IsNullOrWhiteSpace(contentDisposition.FileNameStar.Value);
        }

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                    && contentDisposition.DispositionType.Equals("form-data")
                    && (!string.IsNullOrWhiteSpace(contentDisposition.FileName.Value)
                        || !string.IsNullOrWhiteSpace(contentDisposition.FileNameStar.Value));
        }
    }
}
