using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Models.User;

namespace TravelnessAPI.Models.Tour
{
    public class TourForSightseeingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<DataAccessLayer.Entities.Sightseeing> Sightseeings { get; set; } = new List<DataAccessLayer.Entities.Sightseeing>();
        public UserForTourViewModel User { get; set; }
    }
}
