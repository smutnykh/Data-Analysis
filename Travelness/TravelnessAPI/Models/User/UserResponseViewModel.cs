using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Models.Album;

namespace TravelnessAPI.Models.User
{
    public class UserResponseViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfileInfo { get; set; }
        public string ProfileImage { get; set; }
        public string Role { get; set; }
        public List<DataAccessLayer.Entities.Tour> Tours { get; set; }
        public List<AlbumForUserViewModel> Albums { get; set; }
    }
}
