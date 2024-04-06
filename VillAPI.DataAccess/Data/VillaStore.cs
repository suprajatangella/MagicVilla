using VillaAPI.Models.Dto;

namespace VillAPI.DataAccess.Data
{
    public class VillaStore
    {
        public static List<VilaDTO> villaList= new List<VilaDTO> {
                new VilaDTO { Id = 1, Name ="Pool View", Sqft=100, Occupancy=4},

                new VilaDTO { Id = 2, Name ="Beach View", Sqft=300, Occupancy=3}
            };
    }
}
