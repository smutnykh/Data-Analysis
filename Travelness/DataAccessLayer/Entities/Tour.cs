using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<Sightseeing> Sightseeings { get; set; } = new List<Sightseeing>();
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
