using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Sightseeing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Image { get; set; }
        public List<Tour> Tours { get; set; } = new List<Tour>();
        public List<Rate> Rates { get; set; } = new List<Rate>();
        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
