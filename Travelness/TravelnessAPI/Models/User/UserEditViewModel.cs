using Microsoft.AspNetCore.Http;

namespace TravelnessAPI.Models.User
{
    public class UserEditViewModel
    {
        public string Username { get; set; }
        public string ProfileInfo { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
