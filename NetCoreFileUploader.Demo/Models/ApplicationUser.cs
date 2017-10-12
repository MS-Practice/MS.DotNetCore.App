using Microsoft.AspNetCore.Identity;

namespace NetCoreFileUploader.Demo.Models
{
    public class ApplicationUser:IdentityUser
    {
        public byte[] AvatarImage { get; set; }
    }
}
