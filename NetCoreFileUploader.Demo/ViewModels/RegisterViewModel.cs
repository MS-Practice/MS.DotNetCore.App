using Microsoft.AspNetCore.Http;

namespace NetCoreFileUploader.Demo.ViewModels
{
    public class RegisterViewModel
    {
        public IFormFile AvatarImage { get; set; }

        public string Email { get; set; }
    }
}
