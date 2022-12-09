using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public byte Rating { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SightseeingId { get; set; }
        public Sightseeing Sightseeing { get; set; }
    }
}
