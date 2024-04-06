﻿using System.ComponentModel.DataAnnotations;

namespace VillaAPI.Models.Dto
{
    public class VilaDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public int Occupancy { get; set; }

        public int Sqft { get; set; }

        public string ImageUrl { get; set; }

        public string Amenity { get; set; }

        public string Details { get; set; }

        [Required]
        public string Rate { get; set; }


    }
}
