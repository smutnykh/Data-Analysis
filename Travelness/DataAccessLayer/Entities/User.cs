using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string ProfileInfo { get; set; }
        public string ProfileImage { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public bool IsConfirmed { get; set; }
        public List<Tour> Tours { get; set; } = new List<Tour>();
        public List<Rate> Rates { get; set; } = new List<Rate>();
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
