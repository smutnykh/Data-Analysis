using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelnessAPI.Models.Tour
{
    public class TourCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }
        public List<int> SightseeingsIds { get; set; } = new List<int>();
    }
}
