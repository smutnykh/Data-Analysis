using AutoMapper.Configuration.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelnessAPI.Models.Album
{
    public class AlbumCreateViewModel
    {
        [MapTo("Name")]
        [Required]
        [StringLength(100)]
        public string AlbumName { get; set; }
    }
}
