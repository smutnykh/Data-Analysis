using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TravelnessAPI.Models.Sightseeing
{
    public class SightseeingCreateEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Area { get; set; }
        public IFormFile Image { get; set; }
    }
}
