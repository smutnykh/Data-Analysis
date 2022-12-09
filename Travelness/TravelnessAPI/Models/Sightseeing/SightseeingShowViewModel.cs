using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelnessAPI.Models.Tour;

namespace TravelnessAPI.Models.Sightseeing
{
    public class SightseeingShowViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Image { get; set; }
        public List<TourForSightseeingViewModel> Tours { get; set; } = new List<TourForSightseeingViewModel>();
    }
}
