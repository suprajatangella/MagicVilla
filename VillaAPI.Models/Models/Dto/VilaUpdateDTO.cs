using System.ComponentModel.DataAnnotations;

namespace VillaAPI.Models.Dto
{
    public class VilaUpdateDTO
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public int Sqft { get; set; }
        [Required]

        public string ImageUrl { get; set; }

        public string Amenity { get; set; }

        public string Details { get; set; }

        [Required]
        public string Rate { get; set; }


    }
}
